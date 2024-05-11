using System.IO;
using System.Threading;
using Lindexi.Src.GitCommand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace GitCommand.Tests
{
    [TestClass]
    public class GitTest
    {
        [ContractTestCase]
        public void GetCurrentBranch()
        {
            "获取当前的分支，可以获取到分支名".Test(() =>
            {
                var git = new Git(new DirectoryInfo("."));
                var currentBranch = git.GetCurrentBranch();
                Assert.IsNotNull(currentBranch);
                Assert.AreEqual(false, currentBranch.Contains('\n'));
            });
        }

#if DEBUG
        [ContractTestCase] // 这条有负面效果，不要加入到通用测试里面
#endif
        public void Push()
        {
            "推送给定仓库，可以推送成功".Test(() =>
            {
                var git = new Git(new DirectoryInfo("."));
                Push("origin", "master");
            });
        }

        public string Push(string repository, string branchOrTag, bool force = false)
        {
            var args = $"push \"{repository}\" \"{branchOrTag}\" {(force ? "-f" : "")}";
            return args;
        }

        [ContractTestCase]
        public void GetCurrentCommit()
        {
            "尝试获取当前的 commit 字符串，可以获取成功".Test(() =>
            {
                var git = new Git(new DirectoryInfo("."));
                var currentCommit = git.GetCurrentCommit();
                Assert.IsNotNull(currentCommit);
                Assert.AreEqual(false, currentCommit.Contains('\n'));
            });
        }
    }
}
