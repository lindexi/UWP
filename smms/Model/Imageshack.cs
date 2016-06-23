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
using HttpClient = System.Net.Http.HttpClient;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;


namespace smms.Model
{


    /// <summary>
    /// 图床  sm.ms
    /// </summary>
    public class Imageshack
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        public StorageFile File
        {
            set;
            get;
        }
        /// <summary>
        /// 上传返回
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
        /// 上传图片
        /// </summary>
        public async void UpLoad()
        {
            string url = "https://sm.ms/api/upload";
            //var request= WebRequest.Create(url);
            //request.Method = "POST";
            //request.ContentType = "multipart/form-data; boundary=----WebKitFormBoundarypAIqI1RWBfPWiOKq";
            //request.BeginGetResponse(RespeonseStreamCallback, request);
            //Windows.Web.Http.HttpClient
            // httpClient = new Windows.Web.Http.HttpClient();

            //System.Net.Http.HttpClient httpClient =new HttpClient();
            ////var httpString = new  HttpStringContent("");
            //System.Net.Http.HttpContent content=new StringContent("a");
            ////HttpResponseMessage response =
            ////await http.PostAsync(new Uri(url), http_string);
            //HttpResponseMessage response =
            //    await httpClient.PostAsync(new Uri(url), content);
            //HttpStreamContent httpStreamContent=new HttpStreamContent();


            Windows.Web.Http.HttpClient webHttpClient =
                new Windows.Web.Http.HttpClient();

            //Windows.Web.Http.HttpStringContent httpString=
            //     new HttpStringContent("a");
            // await webHttpClient.PostAsync(new Uri(url), httpString);

            HttpMultipartFormDataContent httpMultipartFormDataContent =
                new HttpMultipartFormDataContent();
            //httpMultipartFormDataContent.Headers.Add("smfile", "1.png");

            var fileContent = new HttpStreamContent(await File.OpenAsync(FileAccessMode.Read));
            fileContent.Headers.Add("Content-Type", "application/octet-stream");
            httpMultipartFormDataContent.Add(fileContent, "smfile", "1.png");
            var str = await webHttpClient.PostAsync(new Uri(url), httpMultipartFormDataContent);
            ResponseString = str.Content.ToString();

            OnUploadedEventHandler?.Invoke(this,ResponseString);
        }

        private void RespeonseStreamCallback(IAsyncResult result)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)result.AsyncState;
                using (Stream stream = httpWebRequest.EndGetRequestStream(result))
                {
                    //发送byte
                    string str = "name=\"smfile\"; filename=\"1.png\"";
                    byte[] buffer = Encoding.UTF8.GetBytes(str);
                    stream.Write(buffer, 0, buffer.Length);
                }
                httpWebRequest.BeginGetResponse(ResponseCallback, httpWebRequest);
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
        }

        private void ResponseCallback(IAsyncResult result)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)result.AsyncState;
            WebResponse webResponse = httpWebRequest.EndGetResponse(result);
            using (Stream stream = webResponse.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                }
            }
        }
    }
}
