// lindexi
// 16:34

using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using lindexi.uwp.ImageShack.Model.Auth.digest;
using lindexi.uwp.ImageShack.Model.RPC;
using Qiniu.Conf;

namespace lindexi.uwp.ImageShack.Model.Auth
{
    public class QiniuAuthClient : Client
    {
        public QiniuAuthClient(Mac mac = null)
        {
            this.mac = mac ?? new Mac();
        }

        public override void SetAuth(HttpWebRequest request, Stream body)
        {
            string pathAndQuery = request.RequestUri.PathAndQuery;
            byte[] pathAndQueryBytes = Config.Encoding.GetBytes(pathAndQuery);
            using (MemoryStream buffer = new MemoryStream())
            {
                string digestBase64 = null;
                if ((request.ContentType == "application/x-www-form-urlencoded")
                    && (body != null))
                {
                    if (!body.CanSeek)
                    {
                        throw new Exception("stream can not seek");
                    }
                    Util.IO.Copy(buffer, body);
                    digestBase64 = SignRequest(request, buffer.ToArray());
                }
                else
                {
                    buffer.Write(pathAndQueryBytes, 0, pathAndQueryBytes.Length);
                    buffer.WriteByte((byte) '\n');
                    digestBase64 = mac.Sign(buffer.ToArray());
                }
                string authHead = "QBox " + digestBase64;
                request.Headers["Authorization"] = authHead;
            }
        }

        protected Mac mac;

        private string SignRequest(HttpWebRequest request, byte[] body)
        {
            Uri url = request.RequestUri;

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
                string digestBase64 = GetSHA1Key(mac.SecretKey, buffer.ToString());
                return mac.AccessKey + ":" + digestBase64;
            }
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