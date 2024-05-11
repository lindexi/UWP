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
    }
}
