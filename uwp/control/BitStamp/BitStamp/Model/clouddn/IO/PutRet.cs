// lindexi
// 16:34

using System;
using System.Collections.Generic;
using System.Diagnostics;
using lindexi.uwp.ImageShack.Model.RPC;
using Newtonsoft.Json;

namespace lindexi.uwp.ImageShack.Model.IO
{
    public class PutRet : CallRet
    {
        public PutRet(CallRet ret)
            : base(ret)
        {
            if (!string.IsNullOrEmpty(Response))
            {
                try
                {
                    Unmarshal(Response);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    this.Exception = e;
                }
            }
        }

        /// <summary>
        ///     如果 uptoken 没有指定 ReturnBody，那么返回值是标准的 PutRet 结构
        /// </summary>
        public string Hash
        {
            get;
            private set;
        }

        /// <summary>
        ///     如果传入的 key == UNDEFINED_KEY，则服务端返回 key
        /// </summary>
        public string key
        {
            get;
            private set;
        }

        private void Unmarshal(string json)
        {
            try
            {
                Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                object temp;
                if (dict.TryGetValue("hash", out temp))
                {
                    Hash = (string) temp;
                }
                if (dict.TryGetValue("key", out temp))
                {
                    //中文不能做上传，如果转的话，在最后返回会把转的百分继续转，也就是需要转一次
                    key = Uri.EscapeDataString((string) temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
