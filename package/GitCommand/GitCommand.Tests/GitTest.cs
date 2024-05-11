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

        [ContractTestCase]
        public void GetLogCommit()
        {
            "获取两个 commit 之间经过的 commit 数量，可以获取成功".Test(() =>
            {
                var git = new Git(new DirectoryInfo("."));
                var commitList = git.GetLogCommit("db25427", "13ca951bb9036999db404991b2ce4c");
                Assert.AreEqual(12, commitList.Length);
            });

            "获取当前的历史提交记录，可以获取成功".Test(() =>
            {
                var git = new Git(new DirectoryInfo("."));
                var commitList = git.GetLogCommit();
                Assert.AreEqual(true, commitList.Length > 0);
            });
        }
    }
}
