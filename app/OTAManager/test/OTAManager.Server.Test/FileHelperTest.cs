using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;
using OTAManager.Server.Core;
using FileHelper = OTAManager.Server.Core.FileHelper;

namespace OTAManager.Server.Test
{
    [TestClass]
    public class FileHelperTest
    {
        [ContractTestCase]
        public void IsSameDrive()
        {
            "传入两个不相同驱动器的文件路径，可以返回不相同的驱动器".Test(() =>
            {
                var file1 = @"C:\lindexi\doubi";
                var file2 = @"D:\doubi";

                Assert.AreEqual(false, FileHelper.IsSameDrive(file1, file2));
            });

            "传入两个相同驱动器的文件路径，可以返回相同的驱动器".Test(() =>
            {
                var file1 = @"C:\lindexi\doubi";
                var file2 = @"C:\doubi";

                Assert.AreEqual(true, FileHelper.IsSameDrive(file1, file2));
            });
        }
    }
}
