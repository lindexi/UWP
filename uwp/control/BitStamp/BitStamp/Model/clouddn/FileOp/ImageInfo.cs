// lindexi
// 16:34

using System.Threading.Tasks;
using lindexi.uwp.ImageShack.Model.RPC;

namespace lindexi.uwp.ImageShack.Model.FileOp
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
