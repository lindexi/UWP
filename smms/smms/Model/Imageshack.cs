// lindexi
// 20:53

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using HttpClient = Windows.Web.Http.HttpClient;

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
            HttpClient webHttpClient =
                new HttpClient();
            HttpMultipartFormDataContent httpMultipartFormDataContent =
                new HttpMultipartFormDataContent();
            var fileContent = new HttpStreamContent(await File.OpenAsync(FileAccessMode.Read));
            fileContent.Headers.Add("Content-Type", "application/octet-stream");
            httpMultipartFormDataContent.Add(fileContent, "smfile", File.Name);
            var str = await webHttpClient.PostAsync(new Uri(url), httpMultipartFormDataContent);
            ResponseString = str.Content.ToString();
            OnUploadedEventHandler?.Invoke(this, ResponseString);
        }
    }
}