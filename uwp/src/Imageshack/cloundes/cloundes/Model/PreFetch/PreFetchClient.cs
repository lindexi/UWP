// lindexi
// 16:34

using System.Threading.Tasks;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.Auth;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.Conf;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.RPC;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.RS;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.PreFetch
{
    /// <summary>
    ///     RS Fetch
    /// </summary>
    internal class PreFetchClient : QiniuAuthClient
    {
        /// <summary>
        ///     Pres the fetch.
        /// </summary>
        /// <returns><c>true</c>, if fetch was pred, <c>false</c> otherwise.</returns>
        /// <param name="path">Path.</param>
        public async Task<bool> PreFetch(EntryPath path)
        {
            string url = Config.PREFETCH_HOST + "/prefetch/" + path.Base64EncodedURI;
            CallRet ret = await Call(url);
            return ret.OK;
        }
    }
}
