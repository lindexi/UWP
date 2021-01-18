using System.Collections.Generic;
using System.IO;

namespace OTAManager.ClientUpdateCore
{
    public class ClientUpdateFileDownloadContext
    {
        public ClientUpdateFileDownloadContext(List<ClientApplicationFileInfo> clientApplicationFileInfoList, DirectoryInfo tempFolder)
        {
            ClientApplicationFileInfoList = clientApplicationFileInfoList;
            TempFolder = tempFolder;
        }

        public List<ClientApplicationFileInfo> ClientApplicationFileInfoList { get; }

        /// <summary>
        /// 用于存放下载内容的临时文件夹
        /// </summary>
        public DirectoryInfo TempFolder { get; }
    }
}
