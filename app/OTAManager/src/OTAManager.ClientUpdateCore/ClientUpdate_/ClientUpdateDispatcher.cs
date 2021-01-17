using System;
using System.IO;
using System.Threading.Tasks;

namespace OTAManager.ClientUpdateCore
{
    /// <summary>
    /// 更新调度器
    /// </summary>
    public class ClientUpdateDispatcher
    {
        public ClientUpdateDispatcher(ClientUpdateManifest clientUpdateManifest)
        {
            ClientUpdateManifest = clientUpdateManifest;
        }

        /// <summary>
        /// 用于存放下载内容的临时文件夹
        /// </summary>
        /// 推荐和安装文件放在相同的分区，这样只需做移动文件
        /// 当然了，对于二进制差分来说，下载到哪都差不多
        public DirectoryInfo TempFolder
        {
            get => _tempFolder ??= Directory.CreateDirectory("Temp");
            set => _tempFolder = value;
        }

        public IClientUpdateFileDownloader? ClientUpdateFileDownloader { set; get; }

        public IClientUpdateInstaller? ClientUpdateInstaller { set; get; }

        public async Task Start()
        {
            // 下载文件
            await Download();

            // 判断安装器是否存在
            await Install();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        private async Task Download()
        {
            var clientUpdateFileDownloader = ClientUpdateFileDownloader ?? new ClientUpdateFileDownloader();

            // 下载这里包含了文件是否正确等的判断
            var result = await clientUpdateFileDownloader.Download(
                  new ClientUpdateFileDownloadContext(ClientUpdateManifest.ClientApplicationFileInfoList, TempFolder));

            if (!result.Success)
            {
                throw new InvalidOperationException($"文件下载失败");
            }
        }

        private async Task Install()
        {
            ClientUpdateInstaller ??= new DefaultClientUpdateInstaller();

            var clientUpdateInstallContext = new ClientUpdateInstallContext(ClientUpdateManifest, TempFolder);
            await ClientUpdateInstaller.Install(clientUpdateInstallContext);
        }

        private ClientUpdateManifest ClientUpdateManifest { get; }

        private DirectoryInfo? _tempFolder;
    }
}
