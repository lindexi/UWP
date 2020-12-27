using System.IO;

namespace OTAManager.ClientUpdateCore
{
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
        public DirectoryInfo TempFolder { get; set; }

        public void Start()
        {
            // 下载文件

            var clientUpdateFileDownloader = new ClientUpdateFileDownloader();
            // 下载这里包含了文件是否正确等的判断
            clientUpdateFileDownloader.Download(ClientUpdateManifest);

            // 判断安装器是否存在
            Install();
        }

        private void Install()
        {
            // 先判断安装文件是否存在

            // 然后运行安装器
            //ClientUpdateManifest.InstallerFileName
        }

        private ClientUpdateManifest ClientUpdateManifest { get; }
    }
}