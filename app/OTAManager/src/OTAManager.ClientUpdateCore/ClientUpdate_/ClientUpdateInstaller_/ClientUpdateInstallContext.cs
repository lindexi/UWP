using System.IO;

namespace OTAManager.ClientUpdateCore
{
    public class ClientUpdateInstallContext
    {
        public ClientUpdateInstallContext(ClientUpdateManifest manifest, DirectoryInfo workFolder)
        {
            var installerFileName = manifest.InstallerFileName;
            var installerArgument = manifest.InstallerArgument;

            ClientUpdateManifest = manifest;

            if (string.IsNullOrEmpty(installerFileName))
            {
                installerFileName = ClientUpdateManifest.DefaultInstallerFileName;
            }

            InstallerFileName = installerFileName;
            InstallerArgument = installerArgument;
            WorkFolder = workFolder;
        }

        public DirectoryInfo WorkFolder { get; }

        /// <summary>
        /// 应用安装器的文件名
        /// </summary>
        public string InstallerFileName { get;} 

        /// <summary>
        /// 用于传给应用安装器的参数
        /// </summary>
        public string? InstallerArgument { get;  }

        public ClientUpdateManifest ClientUpdateManifest { get; }
    }
}
