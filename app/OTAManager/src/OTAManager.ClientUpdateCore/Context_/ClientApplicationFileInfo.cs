namespace OTAManager.ClientUpdateCore
{
    /// <summary>
    /// 客户端应用的文件信息
    /// </summary>
    public class ClientApplicationFileInfo
    {
        /// <summary>
        /// 文件的相对路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件的 md5 内容
        /// </summary>
        public string Md5 { get; set; }

        /// <summary>
        /// 用于下载的链接
        /// </summary>
        /// 后续如果有内容二进制差分的需求，可以采用在服务器端返回特殊的版本
        /// 根据客户端传入的版本，返回特殊的版本，这个特殊的版本下载的文件内容就是二进制差分
        /// 这个方法可以让这里的逻辑简单
        public string DownloadUrl { get; set; }
    }
}
