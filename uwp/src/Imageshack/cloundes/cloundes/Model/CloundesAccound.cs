// lindexi
// 16:34

namespace cloundes.Model
{
    /// <summary>
    ///     配置
    /// </summary>
    public class CloundesAccound : NotifyProperty
    {
        public CloundesAccound()
        {
        }

        public string AccessKey
        {
            set;
            get;
        }

        public string SecretKey
        {
            set;
            get;
        }

        /// <summary>
        ///     空间
        /// </summary>
        public string Bucket
        {
            set;
            get;
        }

        /// <summary>
        ///     域名
        /// </summary>
        public string Url
        {
            set;
            get;
        }

        public bool UploadFileName
        {
            set;
            get;
        }

        /// <summary>
        ///     前缀
        /// </summary>
        public string Pname
        {
            set;
            get;
        }
    }
}