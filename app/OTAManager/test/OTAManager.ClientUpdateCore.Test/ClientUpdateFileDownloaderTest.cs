using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace OTAManager.ClientUpdateCore.Test
{
    [TestClass]
    public class ClientUpdateFileDownloaderTest
    {
        [ContractTestCase]
        public void Download()
        {
            "下载的文件，和给出的 md5 不相同，下载失败".Test(async () =>
            {
                var clientUpdateFileDownloader = new ClientUpdateFileDownloader();
                var downloadFolder = Directory.CreateDirectory(Path.GetRandomFileName());
                var filePath = "Foo\\Installer.exe";

                var clientUpdateFileDownloadContext = new ClientUpdateFileDownloadContext
                (
                    new List<ClientApplicationFileInfo>()
                    {
                        new ClientApplicationFileInfo()
                        {
                            DownloadUrl = "https://10000.gd.cn/10000.gd_speedtest.exe",
                            FilePath = filePath,
                            Md5 = "9f650f3eb7be0a8e82efeb822f53f13a"
                        },
                        new ClientApplicationFileInfo()
                        {
                            DownloadUrl = "https://d.o0o0o0o.cn/Windows6.1-KB2533623-x86.msu.zip",
                            FilePath = "Windows6.1-KB2533623-x86.msu.zip",
                            Md5 = "00000000000000000000000000000000"
                        }
                    },
                    downloadFolder
                );

                var result = await clientUpdateFileDownloader.Download
                (
                    clientUpdateFileDownloadContext
                );

                Assert.AreEqual(false, result.Success);
            });

            "给出多个下载链接和路径，可以全部下载".Test(async () =>
            {
                var clientUpdateFileDownloader = new ClientUpdateFileDownloader();
                var downloadFolder = Directory.CreateDirectory(Path.GetRandomFileName());
                var filePath = "Foo\\Installer.exe";

                var clientUpdateFileDownloadContext = new ClientUpdateFileDownloadContext
                (
                    new List<ClientApplicationFileInfo>()
                    {
                        new ClientApplicationFileInfo()
                        {
                            DownloadUrl = "https://10000.gd.cn/10000.gd_speedtest.exe",
                            FilePath = filePath,
                            Md5 = "9f650f3eb7be0a8e82efeb822f53f13a"
                        },
                        new ClientApplicationFileInfo()
                        {
                            DownloadUrl = "https://d.o0o0o0o.cn/Windows6.1-KB2533623-x86.msu.zip",
                            FilePath = "Windows6.1-KB2533623-x86.msu.zip",
                            Md5 = "edf1d538c85f24ec0ef0991e6b27f0d7"
                        }
                    },
                    downloadFolder
                );

                var result = await clientUpdateFileDownloader.Download
                (
                    clientUpdateFileDownloadContext
                );

                Assert.AreEqual(true, result.Success);

                Assert.AreEqual(true, File.Exists(Path.Combine(downloadFolder.FullName, filePath)));
            });

            "给定下载之后的相对路径，可以下载到相对路径".Test(async () =>
            {
                var clientUpdateFileDownloader = new ClientUpdateFileDownloader();
                var downloadFolder = Directory.CreateDirectory(Path.GetRandomFileName());
                var filePath = "Foo\\Installer.exe";

                var clientUpdateFileDownloadContext = new ClientUpdateFileDownloadContext
                (
                    new List<ClientApplicationFileInfo>()
                    {
                        new ClientApplicationFileInfo()
                        {
                            DownloadUrl = "https://10000.gd.cn/10000.gd_speedtest.exe",
                            FilePath = filePath,
                            Md5 = "9f650f3eb7be0a8e82efeb822f53f13a"
                        }
                    },
                    downloadFolder
                );

                var result = await clientUpdateFileDownloader.Download
                (
                    clientUpdateFileDownloadContext
                );

                Assert.AreEqual(true, result.Success);

                Assert.AreEqual(true, File.Exists(Path.Combine(downloadFolder.FullName, filePath)));
            });

            "下载链接合法存在的文件，可以下载成功".Test(async () =>
            {
                var clientUpdateFileDownloader = new ClientUpdateFileDownloader();
                var result = await clientUpdateFileDownloader.Download
                (
                    new ClientUpdateFileDownloadContext
                    (
                        new List<ClientApplicationFileInfo>()
                        {
                            new ClientApplicationFileInfo()
                            {
                                DownloadUrl = "https://10000.gd.cn/10000.gd_speedtest.exe",
                                FilePath = "Installer.exe",
                                Md5 = "9f650f3eb7be0a8e82efeb822f53f13a"
                            }
                        },
                        Directory.CreateDirectory(Path.GetRandomFileName())
                    )
                );

                Assert.AreEqual(true, result.Success);
            });
        }
    }
}
