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
                git.Push("origin", "master");
            });
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

        [ContractTestCase]
        public void GetGitCommitRevisionCount()
        {
            "获取当前的 git 的 commit 数量，可以获取成功".Test(() =>
            {
                var git = new Git(new DirectoryInfo("."));
                var count = git.GetGitCommitRevisionCount();
                Assert.AreNotEqual(0, count);
            });
        }
    }
}
