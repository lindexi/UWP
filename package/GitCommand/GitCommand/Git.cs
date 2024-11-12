using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Lindexi.Src.GitCommand
{
    public class Git
    {
        static Git()
        {
#if NETCOREAPP
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        }

        /// <inheritdoc />
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
        public List<GitDiffFile> DiffFile(string source, string target)
        {
            var gitDiffFileList = new List<GitDiffFile>();

            return gitDiffFileList;
        }

        public string[] GetLogCommit()
        {
            var (success, control) = RunGitCommand($"log --pretty=format:\"%H\"");

            if (!success)
            {
                return Array.Empty<string>();
            }

            return control.Split('\n');
        }

        public string GetCurrentCommit()
        {
            var (success, output) = RunGitCommand($"rev-parse HEAD");
            if (!success)
            {
                return string.Empty;
            }
            var commit = output.Trim();
            return commit;
        }

        public int GetGitCommitRevisionCount()
        {
            var (success, control) = RunGitCommand("rev-list --count HEAD");

            if (!success)
            {
                return 0;
            }

            var str = control.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(temp => temp.Replace("\r", ""))
                .Where(temp => !string.IsNullOrEmpty(temp)).Reverse().FirstOrDefault();

            if (int.TryParse(str, out var count))
            {
                return count;
            }

            return 0;
        }

        #region Log

        public static string GetFileLastModificationDate(FileInfo file)
        {
            var processStartInfo = new ProcessStartInfo("git")
            {
                RedirectStandardOutput = true, Arguments = $"log -1 --pretty=\"format:%ci\" \"{file.Name}\""
            };

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = true;
            process.StartInfo.WorkingDirectory = file.Directory!.FullName;
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }


        public string[] GetLogCommit(string formCommit, string toCommit)
        {
            var (success, control) = RunGitCommand($"log --pretty=format:\"%H\" {formCommit}..{toCommit}");

            if (!success)
            {
                return Array.Empty<string>();
            }

            return control.Split('\n');
        }

        #endregion


        public void Clone(string repoUrl)
        {
            RunGitCommand($"clone {repoUrl}");
        }

        public static Git Clone(string repoUrl, DirectoryInfo directory)
        {
            var command = $"clone {repoUrl} \"{directory.FullName}\"";
            Console.WriteLine(command);

            var git = @"C:\Program Files\Git\bin\git.exe";
            if (!File.Exists(git))
            {
                git = "git";
            }

            var processStartInfo = new ProcessStartInfo(git, command);
            var process = Process.Start(processStartInfo);
            process.WaitForExit((int) TimeSpan.FromMinutes(10).TotalMilliseconds);

            return new Git(directory);
        }

        public void Clean()
        {
            RunGitCommand("clean -xdf");
        }

        public void FetchAll()
        {
            RunGitCommand("fetch --all --tags");
        }

        public DirectoryInfo Repo { get; }

        private (bool success, string output) RunGitCommand(string command)
        {
            WriteLog($"Start run git command: {command}");
            var result = ExecuteCommand(GetGitPath(), command, Repo.FullName);
            WriteLog($"Finish run git command: {command} ;Success={result.success} ;Output={result.output}");

            return result;
        }

        private static string GetGitPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var git = @"C:\Program Files\Git\bin\git.exe";
                if (File.Exists(git))
                {
                    return git;
                }
            }

            return "git";
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="exeName"></param>
        /// <param name="arguments"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        private static (bool success, string output) ExecuteCommand(string exeName, string arguments,
            string workingDirectory = "")
        {
            if (string.IsNullOrEmpty(workingDirectory))
            {
                workingDirectory = Environment.CurrentDirectory;
            }

            var processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = workingDirectory,
                FileName = exeName,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            using var process = Process.Start(processStartInfo);
            if (process is null)
            {
                return (false, string.Empty);
            }

            var output = process.StandardOutput.ReadToEnd();
            bool success = true;
            if (process.HasExited)
            {
                success = process.ExitCode == 0;
            }

            return (success, output);
        }


        private void WriteLog(string logMessage)
        {
            OnWriteLog(logMessage);
        }

        protected virtual void OnWriteLog(string logMessage)
        {
            if (NeedWriteLog)
            {
                Console.WriteLine(logMessage);
            }
        }

        /// <summary>
        /// 是否需要写入日志
        /// </summary>
        public bool NeedWriteLog { set; get; } = true;

        /// <summary>
        /// 默认命令的超时时间
        /// </summary>
        public TimeSpan DefaultCommandTimeout { set; get; } = TimeSpan.FromMinutes(1);

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

            RunGitCommand(command);
        }

        /// <summary>
        /// 创建新分支，使用 checkout -b <paramref name="branchName"/> 命令
        /// </summary>
        /// <param name="branchName"></param>
        public void CheckoutNewBranch(string branchName)
        {
            RunGitCommand($"checkout -b {branchName}");
        }
    }

    public class GitDiffFile
    {
        /// <inheritdoc />
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
        Added, // A
        Copied, // C
        Deleted, // D
        Modified, // M
        Renamed, // R
        Changed, // T
        Unmerged, // U
        Unknown, // X
    }
}
