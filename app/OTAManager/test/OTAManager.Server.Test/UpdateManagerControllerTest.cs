using System;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;
using OTAManager.Server.Controllers;

namespace OTAManager.Server.Test
{
    [TestClass]
    public class UpdateManagerControllerTest
    {
        [ContractTestCase]
        public void PostTest()
        {
            "请求更新测试的应用，可以收到应用最新版本".Test(async () =>
            {
                var testClient = TestHostBuild.GetTestClient();
                var response = await testClient.PostAsJsonAsync("/UpdateManager", new ApplicationUpdateRequest()
                {
                    ApplicationUpdateInfo = new ApplicationUpdateInfo()
                    {
                        ApplicationId = "123123123123",
                        Version = new Version(1, 0).ToString(),
                    }
                });

                var content = await response.Content.ReadFromJsonAsync<ApplicationUpdateInfo>();
                Assert.AreEqual("123123123123", content.ApplicationId);
            });
        }

        [ContractTestCase]
        public void UpdateTest()
        {
            "更新一个不存在的应用，可以更新成功".Test(async () =>
            {
                var testClient = TestHostBuild.GetTestClient();
                var applicationUpdateInfo = new ApplicationUpdateInfo()
                {
                    ApplicationId = "new", Version = new Version(1, 0).ToString()
                };
                var response = await testClient.PutAsJsonAsync("/UpdateManager", applicationUpdateInfo);
                var latestUpdateInfo = await response.Content.ReadFromJsonAsync<ApplicationUpdateInfo>();

                Assert.AreEqual(applicationUpdateInfo.ApplicationId, latestUpdateInfo.ApplicationId);
                Assert.AreEqual(applicationUpdateInfo.Version, latestUpdateInfo.Version);
            });

            "更新一个存在的应用，可以更新成功".Test(async () =>
            {
                var testClient = TestHostBuild.GetTestClient();
                var applicationUpdateInfo = new ApplicationUpdateInfo()
                {
                    ApplicationId = "new", Version = new Version(2, 0).ToString()
                };
                var response = await testClient.PutAsJsonAsync("/UpdateManager", applicationUpdateInfo);
                var latestUpdateInfo = await response.Content.ReadFromJsonAsync<ApplicationUpdateInfo>();

                Assert.AreEqual(applicationUpdateInfo.ApplicationId, latestUpdateInfo.ApplicationId);
                Assert.AreEqual(applicationUpdateInfo.Version, latestUpdateInfo.Version);
            });
        }
    }
}
