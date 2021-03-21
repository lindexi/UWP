using Microsoft.EntityFrameworkCore;

namespace OTAManager.Server.Model
{
    /// <summary>
    /// 文件存储的数据
    /// </summary>
    [Index(nameof(FileKey))]
    [Index(nameof(Md5))]
    public class FileStorageModel
    {
        public long Id { set; get; }

        public string FileKey { get; set; } = null!;

        /// <summary>
        /// 文件所在磁盘路径
        /// </summary>
        public string FilePath { get; set; } = null!;

        public string Md5 { get; set; } = null!;

        public long FileLength { get; set; }

        //public string Sha1 { get; set; } = null!;
    }
}
