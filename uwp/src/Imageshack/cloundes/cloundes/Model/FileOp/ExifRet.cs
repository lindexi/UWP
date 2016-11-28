// lindexi
// 16:34

using System;
using System.Collections.Generic;
using System.Diagnostics;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.RPC;
using Newtonsoft.Json;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.FileOp
{
    internal class ExifValType
    {
        public string val
        {
            get;
            set;
        }

        public int type
        {
            get;
            set;
        }
    }

    public class ExifRet : CallRet
    {
        public ExifRet(CallRet ret)
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

        internal ExifValType this[string key]
        {
            get
            {
                return dict[key];
            }
        }

        public override string ToString()
        {
            try
            {
                return JsonConvert.SerializeObject(dict).ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private Dictionary<string, ExifValType> dict;

        private void Unmarshal(string json)
        {
            dict = JsonConvert.DeserializeObject<Dictionary<string, ExifValType>>(json);
        }
    }
}