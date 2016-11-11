// lindexi
// 16:34

using System.Threading.Tasks;
using lindexi.uwp.ImageShack.Model.RPC;

namespace lindexi.uwp.ImageShack.Model.FileOp
{
    public static class Exif
    {
        public static string MakeRequest(string url)
        {
            return url + "?exif";
        }

        public static async Task<ExifRet> Call(string url)
        {
            CallRet callRet = await FileOpClient.Get(url);
            return new ExifRet(callRet);
        }
    }
}