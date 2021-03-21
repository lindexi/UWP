using System.IO;
using System.IO.Compression;
using Lindexi.Src.DirectoryToZipStream;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace DirectoryToZipStream.Test
{
    [TestClass]
    public class DirectoryToZipStreamHelperTest
    {
        [ContractTestCase]
        public void ReadDirectoryToZipStream()
        {
            "读取文件夹作为压缩包文件，可以将整个文件夹按照文件顺序放入压缩包".Test(() =>
            {
                // 先创建一个文件夹
                var folder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                // 填充垃圾文件
                var file1Text = "123123";
                var file2Text = "lindexi";
                var file3Text = "doubi";

                var file1Name = "file1.txt";
                var file2Name = @"123\file2.txt";
                var file3Name = @"File\File3.txt";

                var file1Path = Path.Combine(folder, file1Name);
                WriteAllText(file1Path, file1Text);
                var file2Path = Path.Combine(folder, file2Name);
                WriteAllText(file2Path, file2Text);
                var file3Path = Path.Combine(folder, file3Name);
                WriteAllText(file3Path, file3Text);

                var zipFile = Path.GetTempFileName();
                using (var fileStream = File.Create(zipFile))
                {
                    DirectoryToZipStreamHelper.ReadDirectoryToZipStream(new DirectoryInfo(folder), fileStream);
                }

                using var zipStream = File.OpenRead(zipFile);
                var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);
                foreach (var zipArchiveEntry in zipArchive.Entries)
                {
                    if (zipArchiveEntry.FullName == file1Name)
                    {
                        using var streamReader = new StreamReader(zipArchiveEntry.Open());
                        var text = streamReader.ReadToEnd();
                        Assert.AreEqual(file1Text, text);
                    }
                    else if (zipArchiveEntry.FullName == file2Name)
                    {
                        using var streamReader = new StreamReader(zipArchiveEntry.Open());
                        var text = streamReader.ReadToEnd();
                        Assert.AreEqual(file2Text, text);
                    }
                    else if (zipArchiveEntry.FullName == file3Name)
                    {
                        using var streamReader = new StreamReader(zipArchiveEntry.Open());
                        var text = streamReader.ReadToEnd();
                        Assert.AreEqual(file3Text, text);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }

                // 垃圾清理？算了，Win10 下会自己清理的
            });
        }

        private static void WriteAllText(string file, string text)
        {
            var directoryName = Path.GetDirectoryName(file);
            Directory.CreateDirectory(directoryName!);

            File.WriteAllText(file, text);
        }
    }
}
