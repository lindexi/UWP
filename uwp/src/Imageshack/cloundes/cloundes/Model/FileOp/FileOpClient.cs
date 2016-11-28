// lindexi
// 16:34

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.Conf;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.RPC;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.FileOp
{
    internal static class FileOpClient
    {
        public static async Task<CallRet> Get(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "GET";
                request.Headers["User-Agent"] = Config.USER_AGENT;
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    return HandleResult(response);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return new CallRet(HttpStatusCode.BadRequest, e);
            }
        }

        public static CallRet HandleResult(HttpWebResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseStr = reader.ReadToEnd();
                return new CallRet(statusCode, responseStr);
            }
        }
    }
}