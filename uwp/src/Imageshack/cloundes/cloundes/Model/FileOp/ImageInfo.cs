// lindexi
// 16:34

using System;
using System.Threading.Tasks;
using Qiniu.RPC;

namespace Qiniu.FileOp
{
    public static class ImageInfo
    {
        public static string MakeRequest(string url)
        {
            return url + "?imageInfo";
        }

        public static async Task<ImageInfoRet> Call(string url)
        {
            CallRet callRet = await FileOpClient.Get(url);
            return new ImageInfoRet(callRet);
        }
    }
}