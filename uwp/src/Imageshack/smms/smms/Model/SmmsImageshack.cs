using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using Newtonsoft.Json;

namespace lindexi.Imageshack.Smms
{
    /// <summary>
    /// Sm.ms 图床帮助类，提供上传图片到 Smms 图床
    /// </summary>
    public class SmmsImageshack
    {
        /// <summary>
        /// 需要上传的图片文件
        /// </summary>
        /// <param name="storageFile"></param>
        public SmmsImageshack(StorageFile storageFile)
        {
            StorageFile = storageFile;
        }

        /// <summary>
        /// 上传图片到 smms 网站
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="COMException">超时</exception>
        /// <returns></returns>
        public async Task<Smms> UploadImage()
        {
            const string url = "https://sm.ms/api/upload";
            var webHttpClient =
                new HttpClient();
            var httpMultipartFormDataContent =
                new HttpMultipartFormDataContent();

            var fileContent = new HttpStreamContent(await StorageFile.OpenAsync(FileAccessMode.Read));

            fileContent.Headers.Add("Content-Type", "application/octet-stream");

            // 更多 userAgent 请看 win10 uwp 如何让WebView标识win10手机 https://lindexi.gitee.io/post/win10-uwp-%E5%A6%82%E4%BD%95%E8%AE%A9WebView%E6%A0%87%E8%AF%86win10%E6%89%8B%E6%9C%BA.html
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";

            webHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);


            httpMultipartFormDataContent.Add(fileContent, "smfile", StorageFile.Name);

            var response = await webHttpClient.PostAsync(new Uri(url), httpMultipartFormDataContent);

            var responseString = response.Content.ToString();

            Smms smms = JsonConvert.DeserializeObject<Smms>(responseString);
            return smms;
        }

        private StorageFile StorageFile { get; }

        public class Smms
        {
            [JsonProperty(propertyName: "code")]
            public string code { get; set; }
            [JsonProperty(propertyName: "data")]
            public Data data { get; set; }
        }

        public class Data
        {
            [JsonProperty(propertyName: "width")]
            public int width { get; set; }
            [JsonProperty(propertyName: "height")]
            public int height { get; set; }
            [JsonProperty(propertyName: "filename")]
            public string filename { get; set; }
            [JsonProperty(propertyName: "storename")]
            public string storename { get; set; }
            [JsonProperty(propertyName: "size")]
            public int size { get; set; }
            [JsonProperty(propertyName: "path")]
            public string path { get; set; }
            [JsonProperty(propertyName: "hash")]
            public string hash { get; set; }
            [JsonProperty(propertyName: "timestamp")]
            public int timestamp { get; set; }
            [JsonProperty(propertyName: "ip")]
            public string ip { get; set; }
            [JsonProperty(propertyName: "url")]
            public string url { get; set; }
            [JsonProperty(propertyName: "delete")]
            public string delete { get; set; }
        }
    }
}