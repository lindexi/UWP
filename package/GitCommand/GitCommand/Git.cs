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
            var file = Path.GetTempFileName();
            Control($"log --pretty=format:\"%H\" > {file}");

            return File.ReadAllLines(file);
        }

        /// <summary>
        /// 获取当前的 commit 号，将执行 <code>git rev-parse HEAD</code> 命令
        /// </summary>
        /// <returns></returns>
        public string GetCurrentCommit()
        {
            var file = Path.GetTempFileName();
            Control($"rev-parse HEAD > \"{file}\"");
            var commit = File.ReadAllText(file).Trim();
            try
            {
                File.Delete(file);
            }
            catch
            {
                // 清掉失败？那啥也不用做
            }

            return commit;
        }

        /// <summary>
        /// 获取当前的 Git 提交数量，将执行 <code>git rev-list --count HEAD</code> 命令
        /// </summary>
        /// <returns></returns>
        public int GetGitCommitRevisionCount()
        {
            var control = Control("rev-list --count HEAD");
            var str = control.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(temp => temp.Replace("\r", "")).Where(temp => !string.IsNullOrEmpty(temp)).Reverse().FirstOrDefault();

            if (int.TryParse(str, out var count))
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
            var (_, output) = ExecuteCommandWithOutputToFile($"log --pretty=format:\"%H\" {formCommit}..{toCommit}");
            return output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public void Clone(string repoUrl)
        {
            Control($"clone {repoUrl}");
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

        /// <summary>
        /// 使用 git clean -xdf 清理仓库内容，将清理所有没有被追踪的文件
        /// </summary>
        public void Clean()
        {
            Control("clean -xdf");
        }

        /// <summary>
        /// 拉取远程的所有内容，包括了 Tag 号，将执行 <code>git fetch --all --tags</code> 命令
        /// </summary>
        public void FetchAll()
        {
            Control("fetch --all --tags");
        }

        /// <summary>
        /// 对应的 Git 仓库文件夹
        /// </summary>
        public DirectoryInfo Repo { get; }

        private const string GitStr = "git ";

        private string Control(string str)
        {
            str = FileStr() + str;
            WriteLog(str);
            str = Command(str, Repo.FullName);

            WriteLog(str);
            return str;
        }

        private (string commandLineOutput, string commandOutput) ExecuteCommandWithOutputToFile(string command)
        {
            var file = Path.GetTempFileName();
            command = "git " + command + $" >\"{file}\"";
            var commandLineOutput = Command(command, Repo.FullName);
            var commandOutput = string.Empty;

            if (File.Exists(file))
            {
                commandOutput = File.ReadAllText(file);

                try
                {
                    File.Delete(file);
                }
                catch
                {
                    // 清掉失败？那啥也不用做
                }
            }

            return (commandLineOutput, commandOutput);
        }

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

        private string FileStr()
        {
            return string.Format(GitStr, Repo.FullName);
        }

        /// <summary>
        /// 调用命令行的时候的默认编码格式
        /// </summary>
        /// 大部分中文环境开发机上都是用 GBK 编码输出，我这个库也基本上是被我自己使用，设置为 GBK 很合理
        public Encoding StandardOutputEncoding { set; get; } = Encoding.GetEncoding("GBK");

        private string Command(string str, string workingDirectory)
        {
            // string str = Console.ReadLine();
            //System.Console.InputEncoding = System.Text.Encoding.UTF8;//乱码

            Process p = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false, //是否使用操作系统shell启动
                    RedirectStandardInput = true, //接受来自调用程序的输入信息
                    RedirectStandardOutput = true, //由调用程序获取输出信息
                    RedirectStandardError = true, //重定向标准错误输出
                    CreateNoWindow = true, //不显示程序窗口
                    StandardOutputEncoding = StandardOutputEncoding
                }
            };

            p.Start(); //启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令

            bool exited = false;

            //// 超时
            //Task.Run(() =>
            //{
            //    Task.Delay(TimeSpan.FromMinutes(1)).ContinueWith(_ =>
            //    {
            //        if (exited)
            //        {
            //            return;
            //        }

            //        try
            //        {
            //            if (!p.HasExited)
            //            {
            //                Console.WriteLine($"{str} 超时");
            //                p.Kill();
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine(e);
            //        }
            //    });
            //});

            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();
            //Console.WriteLine(output);
            output += p.StandardError.ReadToEnd();
            //Console.WriteLine(output);

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            p.WaitForExit((int) DefaultCommandTimeout.TotalMilliseconds); //等待程序执行完退出进程
            p.Close();

            exited = true;

            return output + "\r\n";
        }


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

            Control(command);
        }

        /// <summary>
        /// 创建新分支，使用 <code>git checkout -b <paramref name="branchName"/></code> 命令
        /// </summary>
        /// <param name="branchName"></param>
        public void CheckoutNewBranch(string branchName)
        {
            Control($"checkout -b {branchName}");
        }

        /// <summary>
        /// 创建新分支，使用 <code>git checkout -b <paramref name="branchName"/></code> 命令
        /// </summary>
        /// <param name="branchName"></param>
        /// <param name="force">是否需要强行创建，加上 -f 命令</param>
        public void CheckoutNewBranch(string branchName, bool force)
        {
            Control($"checkout -{(force ? "B" : "b")} {branchName}");
        }

        /// <summary>
        /// 调用 git add . 命令
        /// </summary>
        public void AddAll()
        {
            Control("add .");
        }

        /// <summary>
        /// 调用 git commit -m message 命令
        /// </summary>
        /// <param name="message"></param>
        public void Commit(string message)
        {
            Control($"commit -m \"{message}\"");
        }

        /// <summary>
        /// 推送代码到仓库，使用 <code>git push {<paramref name="repository"/>} {<paramref name="branchOrTag"/>}</code> 命令
        /// </summary>
        /// <param name="branchOrTag">分支或者是 Tag 号</param>
        /// <param name="repository">参考名，如 origin 仓库</param>
        /// <param name="force">是否需要强行推送，加上 -f 命令</param>
        public string Push(string repository, string branchOrTag, bool force = false)
        {
            return Control($"push \"{repository}\" \"{branchOrTag}\" {(force ? "-f" : "")}");
        }

        /// <summary>
        /// 获取当前分支名，使用 <code>git branch --show-current</code> 命令
        /// </summary>
        /// <returns></returns>
        public string GetCurrentBranch()
        {
            // git rev-parse --abbrev-ref HEAD
            // git branch --show-current （Git 2.22）
            var (_, output) = ExecuteCommandWithOutputToFile("branch --show-current");
            return output.Trim('\n');
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
