// lindexi
// 16:34

using System;
using System.Collections.Generic;
using System.Diagnostics;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.RPC;
using Newtonsoft.Json;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.FileOp
{
    /// <summary>
    ///     Image Info
    /// </summary>
    internal class ImageInfoRet : CallRet
    {
        /// <summary>
        ///     construct
        /// </summary>
        /// <param name="ret"></param>
        public ImageInfoRet(CallRet ret)
            : base(ret)
        {
            if (!String.IsNullOrEmpty(Response))
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
        ///     Width
        /// </summary>
        public int Width
        {
            get;
            private set;
        }

        /// <summary>
        ///     Height
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        /// <summary>
        ///     Format
        /// </summary>
        public string Format
        {
            get;
            private set;
        }

        /// <summary>
        ///     Color Model
        /// </summary>
        public string ColorModel
        {
            get;
            private set;
        }

        private void Unmarshal(string json)
        {
            Dictionary<string, object> dics = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            object tmp;
            if (dics.TryGetValue("format", out tmp))
            {
                this.Format = (string) tmp;
            }
            if (dics.TryGetValue("colorModel", out tmp))
            {
                this.ColorModel = (string) tmp;
            }
            if (dics.TryGetValue("width", out tmp))
            {
                this.Width = Convert.ToInt32(tmp);
            }
            if (dics.TryGetValue("height", out tmp))
            {
                this.Height = Convert.ToInt32(tmp);
            }
        }
    }
}
