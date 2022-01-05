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
            });
        }
    }
}
