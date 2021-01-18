using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OTAManager.Server.Controllers;

namespace OTAManager.ClientUpdateCore
{
    /// <summary>
    /// 应用更新管理
    /// </summary>
    public class ApplicationUpdateManager
    {
        public ApplicationUpdateManager(string host)
        {
            Host = host;
        }

        public ApplicationUpdateManager()
        {
        }

        public string Host { get; set; } = "http://localhost:5000";

        /// <summary>
        /// 获取更新
        /// </summary>
        /// <param name="infoRequest"></param>
        /// <returns></returns>
        public async Task<ApplicationUpdateContext?> GetUpdate(ApplicationUpdateInfoRequest infoRequest)
        {
            var httpClient = GetClient();
            var applicationUpdateInfoText =
                await httpClient.GetStringAsync($"/UpdateManager?applicationId={infoRequest.ApplicationId}");

            if (string.IsNullOrEmpty(applicationUpdateInfoText))
            {
                Log($"找不到应用为 ApplicationId={infoRequest.ApplicationId} 的信息");
                return default;
            }

            var applicationUpdateInfo =
                JsonSerializer.Deserialize<ApplicationUpdateInfo>(applicationUpdateInfoText, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            if (applicationUpdateInfo is null)
            {
                Log($"返回信息找不到 ApplicationUpdateInfo 内容");
                return default;
            }

            return ParseApplicationUpdateInfoModel(applicationUpdateInfo);
        }

        /// <summary>
        /// 调试使用的方法，更新服务器的信息
        /// </summary>
        //[Conditional("DEBUG")]
        public async Task<ApplicationUpdateContext?> UpdateServerInfo(ApplicationUpdateContext context)
        {
            var applicationId = context.ApplicationId;
            var applicationVersion = context.ApplicationVersion;
            var manifest = context.ClientUpdateManifest;

            var httpClient = GetClient();

            var clientUpdateManifestSerializer = new ClientUpdateManifestSerializer();
            var serializedClientUpdateManifest = clientUpdateManifestSerializer.Serialize(manifest);

            var applicationUpdateInfo = new ApplicationUpdateInfo()
            {
                ApplicationId = applicationId,
                Version = applicationVersion.ToString(),
                UpdateContext = serializedClientUpdateManifest
            };

            var response = await httpClient.PutAsJsonAsync("/UpdateManager", applicationUpdateInfo)
                .ConfigureAwait(false);

            var responseApplicationUpdateInfoModel = await response.Content.ReadFromJsonAsync<ApplicationUpdateInfo>();

            if (responseApplicationUpdateInfoModel is null)
            {
                Log($"返回的 ResponseApplicationUpdateInfoModel 是空");
                return default;
            }

            return ParseApplicationUpdateInfoModel(responseApplicationUpdateInfoModel);
        }

        private ApplicationUpdateContext ParseApplicationUpdateInfoModel(ApplicationUpdateInfo model)
        {
            if (!Version.TryParse(model.Version, out var applicationVersion))
            {
                applicationVersion = new Version();
            }
            var applicationUpdateContext = new ApplicationUpdateContext()
            {
                ApplicationId = model.ApplicationId,
                ApplicationVersion = applicationVersion
            };

            if (string.IsNullOrEmpty(model.UpdateContext))
            {
                Log($"没有找到 ApplicationId={model.ApplicationId} 的 UpdateContext 方法");
                return applicationUpdateContext;
            }

            var clientUpdateManifestSerializer = new ClientUpdateManifestSerializer();
            var clientUpdateManifest = clientUpdateManifestSerializer.Deserialize(model.UpdateContext);

            if (clientUpdateManifest is null)
            {
                Log($"无法从 UpdateContext={model.UpdateContext} 转换出 ClientUpdateManifest 对象");
                return applicationUpdateContext;
            }

            applicationUpdateContext.ClientUpdateManifest = clientUpdateManifest;
            return applicationUpdateContext;

            //ApplicationName = clientUpdateManifest.Name;
            //InstallerFileName = clientUpdateManifest.InstallerFileName;
            //InstallerArgument = clientUpdateManifest.InstallerArgument;
            //ClientApplicationFileInfoText = string.Join("\r\n",
            //    clientUpdateManifest.ClientApplicationFileInfoList.Select(temp =>
            //        $"{temp.FilePath}|{temp.DownloadUrl}|{temp.Md5}"));
        }

        private HttpClient GetClient()
        {
            return new HttpClient() { BaseAddress = new Uri(Host) };
        }

        private void Log(string message)
        {
            OnLog?.Invoke(this, message);
        }

        public event EventHandler<string>? OnLog;
    }
}
