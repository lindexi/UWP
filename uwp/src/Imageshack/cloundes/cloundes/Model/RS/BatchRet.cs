// lindexi
// 16:34

using System;
using Newtonsoft.Json;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.RS
{
    internal class BatchRetItem
    {
        public int code;
        public BatchRetData data;
    }

    [JsonObject(MemberSerialization.OptIn)]
    internal class BatchRetData
    {
        /// <summary>
        ///     文件大小.
        /// </summary>
        [JsonProperty("fsize")]
        public Int64 FSize
        {
            get
            {
                return fSize;
            }
            set
            {
                fSize = value;
            }
        }

        /// <summary>
        ///     修改时间.
        /// </summary>
        [JsonProperty("putTime")]
        public Int64 PutTime
        {
            get
            {
                return putTime;
            }
            set
            {
                putTime = value;
            }
        }

        /// <summary>
        ///     文件名.
        /// </summary>
        [JsonProperty("key")]
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance hash.
        /// </summary>
        [JsonProperty("hash")]
        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }

        /// <summary>
        ///     Gets the MIME.
        /// </summary>
        [JsonProperty("mimeType")]
        public string Mime
        {
            get
            {
                return mime;
            }
            set
            {
                mime = value;
            }
        }

        public string EndUser
        {
            get
            {
                return endUser;
            }
            set
            {
                endUser = value;
            }
        }

        /// <summary>
        /// </summary>
        [JsonProperty("error")]
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
            }
        }

        private string endUser;

        private string error;
        private Int64 fSize;

        private string hash;

        private string key;

        private string mime;

        private Int64 putTime;
    }
}
