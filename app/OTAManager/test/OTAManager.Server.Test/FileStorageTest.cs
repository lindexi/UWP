using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;
using OTAManager.Server.Context;
using OTAManager.Server.Controllers;

namespace OTAManager.Server.Test
{
    [TestClass]
    public class FileStorageTest
    {
        [ContractTestCase]
        public void UploadFileTest()
        {
            "上传超长的文件名，可以拿到文件下载链接".Test(async () =>
            {
                var testClient = TestHostBuild.GetTestClient();
                // 本地的文件
                var memoryStream = new MemoryStream();
                for (int i = 0; i < 1000; i++)
                {
                    memoryStream.WriteByte((byte) i);
                }

                memoryStream.Position = 0;

                var multipartFormDataContent = new MultipartFormDataContent();
                var stringContent = new StringContent("12312312312312312312312312312312312312312312312312312312312312312312312312312311111111111111111111111.png");

                multipartFormDataContent.Add(stringContent, "Name");

                var streamContent = new StreamContent(memoryStream);
                multipartFormDataContent.Add(streamContent, "File", "Foo");

                var response = await testClient.PostAsync("/UpdateManager/UploadFile", multipartFormDataContent);

                var key = await response.Content.ReadFromJsonAsync<UploadFileResponse>();

                Assert.AreEqual(true, !string.IsNullOrEmpty(key.DownloadKey));
            });

            "多次上传相同的文件，可以拿到相同的文件下载链接".Test(async () =>
            {
                var testClient = TestHostBuild.GetTestClient();
                // 本地的文件
                var memoryStream = new MemoryStream();
                for (int i = 0; i < 1000; i++)
                {
                    memoryStream.WriteByte((byte) i);
                }

                memoryStream.Position = 0;

                var multipartFormDataContent = new MultipartFormDataContent();
                var stringContent = new StringContent("文件名");
                multipartFormDataContent.Add(stringContent, "Name");

                var streamContent = new StreamContent(memoryStream);
                multipartFormDataContent.Add(streamContent, "File", "Foo");

                var response1 = await testClient.PostAsync("/UpdateManager/UploadFile", multipartFormDataContent);

                var key1 = await response1.Content.ReadFromJsonAsync<UploadFileResponse>();

                memoryStream.Position = 0;
                var response2 = await testClient.PostAsync("/UpdateManager/UploadFile", multipartFormDataContent);

                var key2 = await response2.Content.ReadFromJsonAsync<UploadFileResponse>();

                Assert.AreEqual(key1.DownloadKey, key2.DownloadKey);
            });

            "将本地的文件上传到服务器上，可以拿到对应的链接，链接可以下载文件".Test(async () =>
            {
                var testClient = TestHostBuild.GetTestClient();
                // 本地的文件
                var memoryStream = new MemoryStream();
                for (int i = 0; i < 1000; i++)
                {
                    memoryStream.WriteByte((byte) i);
                }

                memoryStream.Position = 0;

                var multipartFormDataContent = new MultipartFormDataContent();
                var stringContent = new StringContent("文件名");
                multipartFormDataContent.Add(stringContent, "Name");

                var streamContent = new StreamContent(memoryStream);
                multipartFormDataContent.Add(streamContent, "File", "Foo");

                var response = await testClient.PostAsync("/UpdateManager/UploadFile", multipartFormDataContent);

                var key = await response.Content.ReadFromJsonAsync<UploadFileResponse>();

                var downloadResponse = await testClient.GetAsync($"/UpdateManager/DownloadFile?key={key.DownloadKey}");

                var downloadStream = await downloadResponse.Content.ReadAsStreamAsync();
                var stream = new MemoryStream();
                await downloadStream.CopyToAsync(stream);

                stream.Position = 0;
                memoryStream.Position = 0;

                for (int i = 0; i < memoryStream.Length; i++)
                {
                    Assert.AreEqual(memoryStream.ReadByte(), stream.ReadByte());
                }
            });
        }
    }
}
