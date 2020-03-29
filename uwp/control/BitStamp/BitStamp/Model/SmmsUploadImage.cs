using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using Newtonsoft.Json;
using Sm.ms;

namespace BitStamp.Model
{
    public class SmmsUploadImage : UploadImageTask
    {
        public SmmsUploadImage(StorageFile file)
            : base(file)
        {
        }

        public SmmsUploadImage(UploadImageTask uploadImageTask)
            : base(uploadImageTask)
        {
        }

        public string ResponseString
        {
            set;
            get;
        }

        public override async void UploadImage()
        {
            var smms = new Smms("wXfWIIbNHrZrL1kQeL4QqLgClD4OXZu7");

            SmmsInfo smmsInfo = null;

            try
            {
                var str = await smms.UploadImage(await File.OpenStreamForReadAsync(),File.Name);
                ResponseString = str;

                smmsInfo = JsonConvert.DeserializeObject<SmmsInfo>(str);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                OnUploaded?.Invoke(this, false);
                return;
            }

            if (smmsInfo != null)
            {
                if (smmsInfo.Success )
                {
                    Url = smmsInfo.Data.Url;
                    OnUploaded?.Invoke(this, true);
                }
                else
                {
                    OnUploaded?.Invoke(this, false);
                }
            }
            else
            {
                OnUploaded?.Invoke(this, false);
            }
        }


        public class SmmsInfo
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("success")]
            public bool Success { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("code")]
            public string Code { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("message")]
            public string Message { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("data")]
            public Data Data { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("RequestId")]
            public string RequestId { get;set; }

        }

        public class Data
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("file_id")]
            public int FileId { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("width")]
            public int Width { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("height")]
            public int Height { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("filename")]
            public string Filename { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("storename")]
            public string StoreName { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("size")]
            public int Size { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("path")]
            public string Path { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("hash")]
            public string Hash { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("url")]
            public string Url { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("delete")]
            public string Delete { get;set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("page")]
            public string Page { get;set; }
        }
    }
}
