// lindexi
// 16:34

using System;
using Newtonsoft.Json;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.IO.Resumable
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BlkputRet
    {
        [JsonProperty("checksum")]
        public string checkSum;

        [JsonProperty("crc32")]
        public UInt32 crc32;

        [JsonProperty("ctx")]
        public string ctx;

        [JsonProperty("offset")]
        public ulong offset;
    }
}