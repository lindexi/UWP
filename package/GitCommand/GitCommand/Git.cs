#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Lindexi.Src.GitCommand
{
    /// <summary>
    /// 封装命令行调用 Git 执行 Git 命令
    /// </summary>
    public class Git
    {
        static Git()
        {
#if NETCOREAPP
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        }

        /// <summary>
        /// 创建对 Git 命令行调用的封装
        /// </summary>
        /// <param name="repo"></param>
        public Git(DirectoryInfo repo)
        {
            if (ReferenceEquals(repo, null)) throw new ArgumentNullException(nameof(repo));
            if (!Directory.Exists(repo.FullName))
            {
                // 为什么不使用 repo.Exits 因为这个属性默认没有刷新，也就是在创建 DirectoryInfo 的时候文件夹不存在，那么这个值就是 false 即使后续创建了文件夹也不会刷新，需要调用 Refresh 才可以刷新，但是 Refresh 需要修改很多属性
                // 详细请看 https://blog.walterlv.com/post/file-exists-vs-fileinfo-exists.html
                throw new ArgumentException("必须传入存在的文件夹", nameof(repo));
            }

            Repo = repo;
        }

        /// <summary>
        /// 两个版本修改的文件
        /// </summary>
        /// <param name="source">可以传入commit或分支</param>
        /// <param name="target">可以传入commit或分支</param>
        [Obsolete("还没实现")]
        public List<GitDiffFile> DiffFile(string source, string target)
        {
            var gitDiffFileList = new List<GitDiffFile>();

            return gitDiffFileList;
        }

        /// <summary>
        /// 获取当前历史记录的 commit 列表，将执行 <code>git log --pretty=format:"%H"</code> 命令
        /// </summary>
        /// <returns></returns>
        public string[] GetLogCommit()
        {
            var (success, output) = ExecuteCommand("log --pretty=format:\"%H\"");

            if (!success)
            {
                return Array.Empty<string>();
            }

            return output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 获取当前的 commit 号，将执行 <code>git rev-parse HEAD</code> 命令
        /// </summary>
        /// <returns></returns>
        public string GetCurrentCommit()
        {
            var (success, output) = ExecuteCommand("rev-parse HEAD");

            if (!success)
            {
                return string.Empty;
            }

            return output.Trim('\n');
        }

        /// <summary>
        /// 获取当前的 Git 提交数量，将执行 <code>git rev-list --count HEAD</code> 命令
        /// </summary>
        /// <returns></returns>
        public int GetGitCommitRevisionCount()
        {
            var (success, output) = ExecuteCommand("rev-list --count HEAD");

            if (!success)
            {
                return -1;
            }

            if (int.TryParse(output, out var count))
            {
                return count;
            }

            return 0;
        }

        /// <summary>
        /// 获取两个分支或者两个 commit 之间，相差了哪些 commit 号，将执行 <code>git log --pretty=format:"%H" {formCommit}..{toCommit}</code> 命令
        /// </summary>
        /// <param name="formCommit">起始的 commit 或分支</param>
        /// <param name="toCommit">对比的 commit 或分支</param>
        /// <returns></returns>
        public string[] GetLogCommit(string formCommit, string toCommit)
        {
            var (success, output) = ExecuteCommand($"log --pretty=format:\"%H\" {formCommit}..{toCommit}");
            if (!success)
            {
                return Array.Empty<string>();
            }

            return output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void Clone(string repoUrl)
        {
            ExecuteCommand($"clone {repoUrl}");
        }

        public static Git Clone(string repoUrl, DirectoryInfo directory)
        {
            ExecuteCommand($"clone {repoUrl}", directory.FullName);

            return new Git(directory);
        }

        /// <summary>
        /// 使用 git clean -xdf 清理仓库内容，将清理所有没有被追踪的文件
        /// </summary>
        public void Clean()
        {
            ExecuteCommand("clean -xdf");
        }

        /// <summary>
        /// 拉取远程的所有内容，包括了 Tag 号，将执行 <code>git fetch --all --tags</code> 命令
        /// </summary>
        public void FetchAll()
        {
            ExecuteCommand("fetch --all --tags");
        }

        /// <summary>
        /// 对应的 Git 仓库文件夹
        /// </summary>
        public DirectoryInfo Repo { get; }

        private void WriteLog(string str)
        {
            if (NeedWriteLog)
            {
                Console.WriteLine(str);
            }
        }

        /// <summary>
        /// 是否需要写入日志
        /// </summary>
        public bool NeedWriteLog { set; get; } = true;

        /// <summary>
        /// 切换到某个 commit 或分支
        /// </summary>
        public void Checkout(string commit)
        {
            Checkout(commit, false);
        }

        /// <summary>
        /// 切换到某个 commit 或分支
        /// </summary>
        /// <param name="commit"></param>
        /// <param name="shouldHard">是否需要强行切换，加上 -f 命令</param>
        public void Checkout(string commit, bool shouldHard)
        {
            var command = $"checkout {commit}";

            if (shouldHard)
            {
                command += " -f";
            }

            ExecuteCommand(command);
        }

        /// <summary>
        /// 创建新分支，使用 <code>git checkout -b <paramref name="branchName"/></code> 命令
        /// </summary>
        /// <param name="branchName"></param>
        public void CheckoutNewBranch(string branchName)
        {
            ExecuteCommand($"checkout -b {branchName}");
        }

        /// <summary>
        /// 创建新分支，使用 <code>git checkout -b <paramref name="branchName"/></code> 命令
        /// </summary>
        /// <param name="branchName"></param>
        /// <param name="force">是否需要强行创建，加上 -f 命令</param>
        public void CheckoutNewBranch(string branchName, bool force)
        {
            ExecuteCommand($"checkout -{(force ? "B" : "b")} {branchName}");
        }

        /// <summary>
        /// 调用 git add . 命令
        /// </summary>
        public void AddAll()
        {
            ExecuteCommand("add .");
        }

        /// <summary>
        /// 调用 git commit -m message 命令
        /// </summary>
        /// <param name="message"></param>
        public void Commit(string message)
        {
            ExecuteCommand($"commit -m \"{message}\"");
        }

        /// <summary>
        /// 推送代码到仓库，使用 <code>git push {<paramref name="repository"/>} {<paramref name="branchOrTag"/>}</code> 命令
        /// </summary>
        /// <param name="branchOrTag">分支或者是 Tag 号</param>
        /// <param name="repository">参考名，如 origin 仓库</param>
        /// <param name="force">是否需要强行推送，加上 -f 命令</param>
        public string Push(string repository, string branchOrTag, bool force = false)
        {
            var args = $"push \"{repository}\" \"{branchOrTag}\" {(force ? "-f" : "")}";
            var (_, output) = ExecuteCommand(args);
            return output;
        }

        /// <summary>
        /// 获取当前分支名，使用 <code>git branch --show-current</code> 命令
        /// </summary>
        /// <returns></returns>
        public string GetCurrentBranch()
        {
            // git rev-parse --abbrev-ref HEAD
            // git branch --show-current （Git 2.22）
            var (_, output) = ExecuteCommand("branch --show-current");
            return output.Trim('\n');
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private (bool success, string output) ExecuteCommand(string args)
        {
            WriteLog($"git {args}");

            return ExecuteCommand(args, Repo.FullName);
        }

        private static (bool success, string output) ExecuteCommand(string args, string workingDirectory)
        {
            var processStartInfo = new ProcessStartInfo("git", args)
            {
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,

                CreateNoWindow = true,
                UseShellExecute = false,
            };
            var process = new Process()
            {
                StartInfo = processStartInfo,
            };

            var outputList = new List<string?>();
            var errorList = new List<string?>();

            process.OutputDataReceived += (sender, eventArgs) =>
            {
                outputList.Add(eventArgs.Data);
            };

            process.ErrorDataReceived += (sender, eventArgs) =>
            {
                errorList.Add(eventArgs.Data);
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
            int exitCode;
            try
            {
                Debug.Assert(process.HasExited);
                exitCode = process.ExitCode;
            }
            catch (Exception)
            {
                // 也许有些进程拿不到
                exitCode = errorList.Count > 0 ? -1 : 0;
            }

            if (outputList.Count > 0)
            {
                if (outputList[^1] is null)
                {
                    outputList.RemoveAt(outputList.Count - 1);
                }
            }

            var output = string.Join('\n', outputList);
            return (exitCode == 0, output);
        }
    }

    public class GitDiffFile
    {
        public GitDiffFile(DiffType diffType, FileInfo file)
        {
            DiffType = diffType;
            File = file;
        }

        public DiffType DiffType { get; }
        public FileInfo File { get; }
    }

    public enum DiffType
    {
        Added,// A
        Copied,// C
        Deleted,// D
        Modified,// M
        Renamed,// R
        Changed,// T
        Unmerged,// U
        Unknown,// X
    }
}
