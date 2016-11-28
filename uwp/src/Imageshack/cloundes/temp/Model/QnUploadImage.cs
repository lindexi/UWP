// lindexi
// 16:34

using System;
using Windows.Storage;
using lindexi.uwp.ImageShack.Model.IO;

namespace lindexi.uwp.ImageShack.Model
{
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

        public CloundesAccound Accound
        {
            set;
            get;
        }


        public override async void UploadImage()
        {
            if (Accound == null)
            {
                //默认
                Accound = AppId.Accound;
            }
            //判断域名最后存在“/”不存在就加，不然会看不到域名和图片
            if (!Accound.Url.EndsWith("/"))
            {
                Accound.Url += "/";
            }
            string name = null;
            if (string.IsNullOrEmpty(Name))
            {
                if (Accound.UploadFileName)
                {
                    name = File.Name;
                }
            }
            else
            {
                name = Name;
            }

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(Accound.Pname))
            {
                name = Accound.Pname + name;
            }

            try
            {
                IOClient upload = new IOClient();

                PutRet temp = await upload.UploadFile(
                    Accound.AccessKey,
                    Accound.SecretKey,
                    Accound.Bucket,
                    File, name);

                Url = Accound.Url + temp.key;

                OnUploaded?.Invoke(this, true);
            }
            catch (Exception)
            {
                OnUploaded?.Invoke(this, false);
            }
        }

        ///// <param name="accessKey"></param>
        ///// </summary>
        ///// 上传文件

        ///// <summary>
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