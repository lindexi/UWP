// lindexi
// 10:19

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.Auth.digest;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.Conf;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.RPC;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.RS;
using Windows.Storage;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.IO
{
    /// <summary>
    ///     上传客户端
    /// </summary>
    public class IOClient
    {
        public IOClient()
        {
        }

        /// <summary>
        ///     无论成功或失败，上传结束时触发的事件
        /// </summary>
        public event EventHandler<PutRet> OnPutFinished;

        protected void PutFinished(PutRet ret)
        {
            OnPutFinished?.Invoke(this, ret);
        }

        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="bucket"></param>
        /// <param name="file"></param>
        /// <param name="name">文件名</param>
        /// <returns></returns>
        public async Task<PutRet> UploadFile(string accessKey,
            string secretKey,
            string bucket,
            StorageFile file,
            string name = null)
        {
            Mac mac = new Mac(accessKey, Config.Encoding.GetBytes(secretKey));
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SetExpires(3600);

            string uploadToken = mac.CreateUploadToken(putPolicy);

            Stream stream = await file.OpenStreamForReadAsync();

            var ret = await new IOClient().Put(uploadToken, name, stream, new PutExtra());

            stream.Dispose();

            return ret;
        }

        /// <summary>
        ///     设置连接代理
        /// </summary>
        private IWebProxy Proxy
        {
            set;
            get;
        }


        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="upToken"></param>
        /// <param name="key"></param>
        /// h
        /// <param name="localFile"></param>
        /// <param name="extra"></param>
        private async Task<PutRet> PutFile(string upToken, string key, string localFile, PutExtra extra)
        {
            if (!File.Exists(localFile))
            {
                throw new Exception(string.Format("{0} does not exist", localFile));
            }
            //PutRet ret;

            /*NameValueCollection*/
            WebHeaderCollection formData = GetFormData(upToken, key, extra);
            try
            {
                CallRet callRet = await MultiPart.MultiPost(Config.UP_HOST, formData, localFile, this.Proxy);
                var ret = new PutRet(callRet);
                PutFinished(ret);
                return ret;
            }
            catch (Exception e)
            {
                var ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
                PutFinished(ret);
                return ret;
            }
        }

        /// <summary>
        ///     Puts the file without key.
        /// </summary>
        /// <returns>The file without key.</returns>
        /// <param name="upToken">Up token.</param>
        /// <param name="localFile">Local file.</param>
        /// <param name="extra">Extra.</param>
        private async Task<PutRet> PutFileWithoutKey(string upToken, string localFile, PutExtra extra)
        {
            return await PutFile(upToken, null, localFile, extra);
        }

        /// <summary>
        /// </summary>
        /// <param name="upToken">Up token.</param>
        /// <param name="key">Key.</param>
        /// <param name="putStream">Put stream.</param>
        /// <param name="extra">Extra.</param>
        private async Task<PutRet> Put(string upToken, string key, Stream putStream, PutExtra extra)
        {
            if (!putStream.CanRead)
            {
                throw new Exception("read put Stream error");
            }
            //PutRet ret;
            /*NameValueCollection*/
            WebHeaderCollection formData = GetFormData(upToken, key, extra);
            try
            {
                CallRet callRet = await MultiPart.MultiPost(Config.UP_HOST, formData, putStream);
                var ret = new PutRet(callRet);
                PutFinished(ret);
                return ret;
            }
            catch (Exception e)
            {
                var ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
                PutFinished(ret);
                return ret;
            }
        }

        private static WebHeaderCollection /*NameValueCollection*/ GetFormData(string upToken,
            string key,
            PutExtra extra)
        {
            //System.Collections.Specialized
            WebHeaderCollection webHeader = new WebHeaderCollection
            {
                ["token"] = upToken
            };
            //NameValueCollection formData = new NameValueCollection();
            //formData["token"] = upToken;

            if (key != null)
            {
                webHeader["key"] = key;
                //formData["key"] = key;
            }
            if (extra != null)
            {
                if (extra.CheckCrc == CheckCrcType.CHECK_AUTO)
                {
                    webHeader["crc32"] = extra.Crc32.ToString();
                    //formData["crc32"] = extra.Crc32.ToString();
                }
                if (extra.Params != null)
                {
                    foreach (KeyValuePair<string, string> pair in extra.Params)
                    {
                        webHeader[pair.Key] = pair.Value;
                        //formData[pair.Key] = pair.Value;
                    }
                }
            }
            return webHeader;
            //return formData;
        }
    }
}
