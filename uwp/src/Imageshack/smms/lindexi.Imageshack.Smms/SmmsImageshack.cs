using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

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
            StorageFile = storageFile ?? throw new ArgumentNullException(nameof(storageFile));
        }

        /// <summary>
        /// 需要上传的图片文件
        /// </summary>
        public SmmsImageshack(string fileName, Stream file)
        {
            _stream = file ?? throw new ArgumentNullException(nameof(file));

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            _fileName = fileName;
        }


        /// <summary>
        /// 上传图片到 smms 网站
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="COMException">超时</exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <returns></returns>
        public async Task<Smms> UploadImage()
        {
            if (StorageFile != null)
            {
                var stream = (await StorageFile.OpenAsync(FileAccessMode.Read)).AsStream();
                var fileName = StorageFile.Name;
                return await UploadImageInternal(stream, fileName);
            }
            else
            {
                return await UploadImageInternal(_stream, _fileName);
            }
        }

        private async Task<Smms> UploadImageInternal(Stream file, string fileName)
        {
            const string url = "https://sm.ms/api/upload";
            var httpClient =
                new HttpClient();
            var userAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            await httpClient.PostAsync(new Uri(url), new StringContent(""));

            var multipartFormDataContent = new MultipartFormDataContent();
            var fileContent = new StreamContent(file);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            multipartFormDataContent.Add(fileContent, "smfile", fileName);

            var response = await httpClient.PostAsync(new Uri(url), multipartFormDataContent);
            var str = await response.Content.ReadAsStringAsync();

            var smms = JsonConvert.DeserializeObject<Smms>(str);
            return smms;
        }

        private StorageFile StorageFile { get; }

        private readonly Stream _stream;
        private readonly string _fileName;


        public class Smms
        {
            /// <summary>
            /// 上传文件状态。正常情况为 success。出现错误时为 error
            /// </summary>
            [JsonProperty(propertyName: "code")]
            public string Code { get; set; }

            [JsonProperty(propertyName: "data")]
            public Data Data { get; set; }
        }

        public class Data
        {
            /// <summary>
            /// 图片的宽度
            /// </summary>
            [JsonProperty(propertyName: "width")]
            public int Width { get; set; }

            /// <summary>
            /// 图片的高度
            /// </summary>
            [JsonProperty(propertyName: "height")]
            public int Height { get; set; }

            /// <summary>
            /// 上传文件时所用的文件名
            /// </summary>
            [JsonProperty(propertyName: "filename")]
            public string Filename { get; set; }

            /// <summary>
            /// 上传后的文件名
            /// </summary>
            [JsonProperty(propertyName: "storename")]
            public string Storename { get; set; }

            /// <summary>
            /// 文件大小
            /// </summary>
            [JsonProperty(propertyName: "size")]
            public int Size { get; set; }

            /// <summary>
            /// 图片的相对地址
            /// </summary>
            [JsonProperty(propertyName: "path")]
            public string Path { get; set; }

            /// <summary>
            /// 随机字符串，用于删除文件
            /// </summary>
            [JsonProperty(propertyName: "hash")]
            public string Hash { get; set; }

            [JsonProperty(propertyName: "timestamp")]
            public int Timestamp { get; set; }

            [JsonProperty(propertyName: "ip")]
            public string Ip { get; set; }

            /// <summary>
            /// 图片服务器地址
            /// </summary>
            [JsonProperty(propertyName: "url")]
            public string Url { get; set; }

            /// <summary>
            /// 删除上传的图片文件专有链接
            /// </summary>
            [JsonProperty(propertyName: "delete")]
            public string Delete { get; set; }
        }
    }
}
