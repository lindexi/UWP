// lindexi
// 15:43

using System;
using System.IO;
using System.Net;
using Qiniu.Conf;
using Qiniu.Util;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Newtonsoft.Json;
using Qiniu.RS;

namespace Qiniu.Auth.digest
{
    /// <summary>
    ///     七牛消息认证(Message Authentication)
    /// </summary>
    public class Mac
    {
        public Mac()
        {
            this.AccessKey = Config.ACCESS_KEY;
            this._secretKey = Config.Encoding.GetBytes(Config.SECRET_KEY);
        }

        public Mac(string access, byte[] secretKey)
        {
            this.AccessKey = access;
            this._secretKey = secretKey;
        }

        /// <summary>
        ///     Gets or sets the access key.
        /// </summary>
        /// <value>The access key.</value>
        public string AccessKey
        {
            set;
            get;
        }

        /// <summary>
        ///     Gets the secret key.
        /// </summary>
        /// <value>The secret key.</value>
        public byte[] SecretKey => _secretKey;

        public string CreateUploadToken(PutPolicy putPolicy)
        {
            return CreateUploadToken(putPolicy, this);
        }

        public static string CreateUploadToken(PutPolicy putPolicy, Mac mac)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            string jsonData = JsonConvert.SerializeObject(putPolicy, setting);

            return mac.SignWithData(Encoding.UTF8.GetBytes(jsonData));
        }

        //public string CreateManageToken(string url, byte[] reqBody)
        //{
        //    return CreateManageToken(url, reqBody, this);
        //}

        //public static string CreateManageToken(string url, byte[] reqBody, Mac mac)
        //{
        //    return string.Format("QBox {0}", mac.SignRequest(url, reqBody));
        //}

        public string CreateDownloadToken(string rawUrl)
        {
            return CreateDownloadToken(rawUrl, this);
        }

        public static string CreateDownloadToken(string rawUrl, Mac mac)
        {
            return mac.Sign(Encoding.UTF8.GetBytes(rawUrl));
        }

        /// <summary>
        ///     Sign
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string Sign(byte[] b)
        {
            return string.Format("{0}:{1}", this.AccessKey, _sign(b));
        }

        //public string SignRequest(string url, byte[] reqBody)
        //{
        //    Uri u = new Uri(url);
        //    using (HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(this.SecretKey)))
        //    {
        //        string pathAndQuery = u.PathAndQuery;
        //        byte[] pathAndQueryBytes = Encoding.UTF8.GetBytes(pathAndQuery);
        //        using (MemoryStream buffer = new MemoryStream())
        //        {
        //            buffer.Write(pathAndQueryBytes, 0, pathAndQueryBytes.Length);
        //            buffer.WriteByte((byte)'\n');
        //            if (reqBody != null && reqBody.Length > 0)
        //            {
        //                buffer.Write(reqBody, 0, reqBody.Length);
        //            }
        //            byte[] digest = hmac.ComputeHash(buffer.ToArray());
        //            string digestBase64 = Qiniu.Util.Base64URLSafe.urlSafeBase64Encode(digest);
        //            return string.Format("{0}:{1}", this.AccessKey, digestBase64);
        //        }
        //    }
        //}

        /// <summary>
        ///     SignWithData
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string SignWithData(byte[] b)
        {
            string data = Base64URLSafe.Encode(b);
            return string.Format("{0}:{1}:{2}", this.AccessKey, _sign(Config.Encoding.GetBytes(data)), data);
        }

        /// <summary>
        ///     SignRequest
        /// </summary>
        /// <param name="request"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public string SignRequest(HttpWebRequest request, byte[] body)
        {
            Uri u = request.RequestUri;

            string pathAndQuery = request.RequestUri.PathAndQuery;
            byte[] pathAndQueryBytes = Config.Encoding.GetBytes(pathAndQuery);
            using (MemoryStream buffer = new MemoryStream())
            {
                buffer.Write(pathAndQueryBytes, 0, pathAndQueryBytes.Length);
                buffer.WriteByte((byte) '\n');
                if (body.Length > 0)
                {
                    buffer.Write(body, 0, body.Length);
                }
                string digestBase64 = GetSHA1Key(SecretKey, buffer.ToString());
                return this.AccessKey + ":" + digestBase64;
            }
        }

        private byte[] _secretKey;

        /// <summary>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string _sign(byte[] data)
        {
            return GetSHA1Key(SecretKey, Config.Encoding.GetString(data));
        }

        private string GetSHA1Key(byte[] secretKey, string value)
        {
            var objMacProv = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
            var hash = objMacProv.CreateHash(secretKey.AsBuffer());
            hash.Append(CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8));
            return CryptographicBuffer.EncodeToBase64String(hash.GetValueAndReset()).Replace('+', '-').Replace('/', '_');
        }
    }
}