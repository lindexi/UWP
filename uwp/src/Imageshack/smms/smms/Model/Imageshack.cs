// lindexi
// 20:53

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using HttpClient = System.Net.Http.HttpClient;

namespace smms.Model
{
    /// <summary>
    ///     图床  sm.ms
    /// </summary>
    public class Imageshack
    {
        /// <summary>
        ///     上传文件
        /// </summary>
        public StorageFile File
        {
            set;
            get;
        }

        /// <summary>
        ///     上传返回
        /// </summary>
        public string ResponseString
        {
            set;
            get;
        }

        public EventHandler<string> OnUploadedEventHandler
        {
            set;
            get;
        }

        /// <summary>
        ///     上传图片
        /// </summary>
        public async void UpLoad()
        {
            string url = "https://sm.ms/api/upload";
            HttpClient httpClient =
                new HttpClient();

            var multipartFormDataContent = new MultipartFormDataContent();
            var fileContent = new StreamContent((await File.OpenAsync(FileAccessMode.Read)).AsStream());
            fileContent.Headers.ContentType=new MediaTypeHeaderValue("application/octet-stream");
            multipartFormDataContent.Add(fileContent,"smfile",File.Name);
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            var response =await httpClient.PostAsync(new Uri(url),multipartFormDataContent);
            var str = await response.Content.ReadAsStringAsync();

            //HttpMultipartFormDataContent httpMultipartFormDataContent =
            //    new HttpMultipartFormDataContent();
            //var fileContent = new HttpStreamContent(await File.OpenAsync(FileAccessMode.Read));
            //fileContent.Headers.Add("Content-Type", "application/octet-stream");
            //httpMultipartFormDataContent.Add(fileContent, "smfile", File.Name);


            //var str = await webHttpClient.PostAsync(new Uri(url), httpMultipartFormDataContent);
            //ResponseString = str.Content.ToString();
            //OnUploadedEventHandler?.Invoke(this, ResponseString);
        }
    }
}