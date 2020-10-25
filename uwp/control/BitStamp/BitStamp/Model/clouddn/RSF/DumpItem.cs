// lindexi
// 16:34

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace lindexi.uwp.ImageShack.Model.RSF
{
    /// <summary>
    ///     Dump item文件信息.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DumpItem
    {
        /// <summary>
        ///     文件大小.
        /// </summary>
        [JsonProperty("fsize")]
        public Int64 FSize
        {
            set;
            get;
        }

        /// <summary>
        ///     修改时间.
        /// </summary>
        [JsonProperty("putTime")]
        public Int64 PutTime
        {
            set;
            get;
        }

        /// <summary>
        ///     文件名.
        /// </summary>
        [JsonProperty("key")]
        public string Key
        {
            set;
            get;
        }

        /// <summary>
        ///     Gets a value indicating whether this instance hash.
        /// </summary>
        [JsonProperty("hash")]
        public string Hash
        {
            set;
            get;
        }

        /// <summary>
        ///     Gets the MIME.
        /// </summary>
        [JsonProperty("mimeType")]
        public string Mime
        {
            set;
            get;
        }

        public string EndUser
        {
            set;
            get;
        }
    }

    /// <summary>
    ///     Fetch 返回结果.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DumpRet
    {
        /// <summary>
        ///     fetch 定位符
        /// </summary>
        [JsonProperty("marker")]
        public string Marker
        {
            set;
            get;
        }

        /// <summary>
        ///     The items.
        /// </summary>
        [JsonProperty("items")]
        public List<DumpItem> Items
        {
            set;
            get;
        }
    }
}
