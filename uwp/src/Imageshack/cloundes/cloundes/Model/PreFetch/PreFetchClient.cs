// lindexi
// 16:34

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Qiniu.Auth;
using Qiniu.Conf;
using Qiniu.RPC;
using Qiniu.RS;

namespace Qiniu.PreFetch
{
    /// <summary>
    ///     RS Fetch
    /// </summary>
    public class PreFetchClient : QiniuAuthClient
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