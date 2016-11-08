using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using BitStamp.Model;
using Qiniu.Auth.digest;
using Qiniu.IO;
using Qiniu.RS;

namespace cloundes.Model
{
    /// <summary>
    /// 配置
    /// </summary>
    public class CloundesAccound
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
        /// 空间
        /// </summary>
        public string Bucket
        {
            set;
            get;
        }
        /// <summary>
        /// 域名
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
        /// 前缀
        /// </summary>
        public string Pname
        {
            set;
            get;
        }
    }

    public class QnUploadImage : UploadImageTask
    {
        public QnUploadImage(StorageFile file)
            : base(file)
        {
        }

        public QnUploadImage(UploadImageTask uploadImageTask)
            : base(uploadImageTask)
        {
        }



        public override async void UploadImage()
        {
            if (Accound == null)
            {
                //默认
                Accound = AppId.Accound;
            }

            IOClient upload = new IOClient();
            PutRet temp = await upload.UploadFile(
                Accound.AccessKey,
                Accound.SecretKey,
                Accound.Bucket,
                File);
        }

        public CloundesAccound Accound
        {
            set;
            get;
        }

        ///// <summary>
        ///// 上传文件
        ///// </summary>
        ///// <param name="accessKey"></param>
        ///// <param name="secretKey"></param>
        ///// <param name="bucket"></param>
        ///// <param name="file"></param>
        ///// <param name="name">文件名</param>
        ///// <returns></returns>
        //public async Task<PutRet> UploadFile(string accessKey,
        //    string secretKey,
        //    string bucket,
        //    StorageFile file,
        //    string name = null)
        //{
        //    Mac mac = new Mac(accessKey, Qiniu.Conf.Config.Encoding.GetBytes(secretKey));
        //    PutPolicy putPolicy = new PutPolicy();
        //    putPolicy.Scope = bucket;
        //    putPolicy.SetExpires(3600);

        //    string uploadToken = mac.CreateUploadToken(putPolicy);

        //    Stream stream = await file.OpenStreamForReadAsync();

        //    return await new Qiniu.IO.IOClient().Put(uploadToken, name, stream, new PutExtra());
        //}
    }
}
