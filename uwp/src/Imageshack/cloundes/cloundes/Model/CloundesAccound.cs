// lindexi
// 16:34

using cloundes;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model
{
    /// <summary>
    ///     配置
    /// </summary>
    public class CloundesAccound : NotifyProperty
    {
        public CloundesAccound()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="bucket">空间名</param>
        /// <param name="url">域名</param>
        /// <param name="uploadFileName">是否要上传文件名称，如果false，为随机名称</param>
        /// <param name="pname">文件名前缀</param>
        public CloundesAccound(string accessKey,
            string secretKey,
            string bucket, string url,
            bool uploadFileName = true, string pname = null)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            Bucket = bucket;
            Url = url;
            UploadFileName = uploadFileName;
            Pname = pname;
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
