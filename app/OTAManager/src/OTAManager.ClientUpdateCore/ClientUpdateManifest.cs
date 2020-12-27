using System.Collections.Generic;

namespace OTAManager.ClientUpdateCore
{
    /// <summary>
    /// 客户端更新清单
    /// </summary>
    /// 设计为 xml 格式，太旧的客户端也能兼容，当然了太旧的也使用了旧的接口，因此作用不大
    public class ClientUpdateManifest
    {
        /// <summary>
        /// 应用名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 客户端应用的文件信息
        /// </summary>
        /// 这里会约定存在一个叫 <see cref="InstallerFileName"/> 的文件，这个文件就在下载更新包完成之后被调用
        /// 通过 Installer.exe 这个文件完成所有的更新
        /// 如果是二进制差分的，在 Installer.exe 这个文件里面完成拼装等行为
        public List<ClientApplicationFileInfo> ClientApplicationFileInfos { get; set; }

        /// <summary>
        /// 应用安装器的文件名
        /// </summary>
        public string InstallerFileName { get; set; } = DefaultInstallerFileName;

        /// <summary>
        /// 用于传给应用安装器的参数
        /// </summary>
        public string InstallerArgument { get; set; }

        public const string DefaultInstallerFileName = "Installer.exe";
    }
}
