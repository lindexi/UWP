using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using HttpClient = System.Net.Http.HttpClient;

namespace BitStamp.Model.Cimage
{
    public class Cimage : UploadImageTask
    {
        public Cimage(StorageFile file) : base(file)
        {
        }

        public Cimage(UploadImageTask uploadImageTask) : base(uploadImageTask)
        {
        }

        public override void UploadImage()
        {
            GetCookie();
            CookieAwareWebClientAsync();
        }

        private async void CookieAwareWebClientAsync()
        {
            AccountCimage account = AppId.AccoutCimage;
            var url = new Uri("https://passport.csdn.net/account/login");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded";
            CookieContainer cookie = new CookieContainer();
            request.CookieContainer = cookie;
            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());

            cookie = request.CookieContainer;
            string sc = cookie.GetCookieHeader(url);

            string str = "";
            using (StreamReader stream= new StreamReader(response.GetResponseStream()))
            {
                str = stream.ReadToEnd();
            }


            Regex regex = new Regex(" type=\"hidden\" name=\"lt\" value=\"([\\w|\\-]+)\"");
            var lt = regex.Match(str).Groups[1].Value;
            regex = new Regex("type=\"hidden\" name=\"execution\" value=\"(\\w+)\"");
            var execution = regex.Match(str).Groups[1].Value;

            str = $"username={account.UserName}&password={account.Key}&lt={lt}&execution={execution}&_eventId=submit";
            //str = str.Replace("@", "%40");
            str = WebUtility.UrlEncode(str);

            request.Method = "post";
            byte[] postby = Encoding.UTF8.GetBytes(str);

        }

        private async void GetCookie()
        {
            AccountCimage account = AppId.AccoutCimage;
            //https://stackoverflow.com/questions/41599384/httpclient-cookie-issue
            CookieContainer cookies = new CookieContainer();

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            HttpClient http = new HttpClient(handler);
            var url = new Uri("https://passport.csdn.net/account/login");
            //https://passport.csdn.net/?service=http://write.blog.csdn.net/
            //HttpRequestHeader.
            //http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"));
            //http.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip, deflate, br"));
            //http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36"));

            http.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            //http.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "zh-CN,zh;q=0.8");
            http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");


            handler.UseCookies = true;
            handler.AllowAutoRedirect = true;

            string str = await http.GetStringAsync(url);

            HandlCookie(handler, url);

            Regex regex = new Regex(" type=\"hidden\" name=\"lt\" value=\"([\\w|\\-]+)\"");
            var lt = regex.Match(str).Groups[1].Value;
            regex = new Regex("type=\"hidden\" name=\"execution\" value=\"(\\w+)\"");
            var execution = regex.Match(str).Groups[1].Value;

            str = $"username={account.UserName}&password={account.Key}&lt={lt}&execution={execution}&_eventId=submit";
            str = str.Replace("@", "%40");

            HttpContent content = new StringContent(str, Encoding.UTF8);

            //((StringContent)content).Headers
            //username=lindexi_gd%40163.com&password=Huc_3113006277&lt=LT-541277-RgRpxUYUjiMNRYpXclGBNBy0pInajL&execution=e10s1&_eventId=submit
            //username=lindexi_gd%40163.com&password=Huc_3113006277&lt=LT-541546-HzhKAKoaeftqtL6EtiCQYXrC3d716w&execution=e1s1&_eventId=submit
            str = await content.ReadAsStringAsync();
            //content=new
            content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username",account.UserName),//.Replace("@", "%40")),
                new KeyValuePair<string, string>("password",account.Key),
                new KeyValuePair<string, string>("lt",lt),
                new KeyValuePair<string, string>("execution",execution),
                new KeyValuePair<string, string>("_eventId","submit")
            });
            str = await content.ReadAsStringAsync();

            str = await (await http.PostAsync(url, content)).Content.ReadAsStringAsync();
            Debug.WriteLine(str);
            HandlCookie(handler,url);
            //str = await http.GetStringAsync(url);
            //cookies = new CookieContainer();
            //cookies.MaxCookieSize = int.MaxValue;
            //foreach (Cookie temp in handler.CookieContainer.GetCookies(url))
            //{
            //    try
            //    {
            //        //temp.Domain = ".csdn.net";
            //        //if ("http://write.blog.csdn.net/".Contains(temp.Domain))
            //        //{
            //        //    cookies.SetCookies(new Uri("http://write.blog.csdn.net/"), temp.ToString());
            //        //}
            //    }
            //    catch
            //    {

            //    }
            //}

            //handler.CookieContainer.Add(new Uri("http://write.blog.csdn.net/"), cookies.GetCookies(new Uri("http://write.blog.csdn.net/")));

            //foreach (Cookie temp in cookies.GetCookies(new Uri("http://write.blog.csdn.net/")))
            //{
            //    handler.CookieContainer.Add();
            //}

            //var temp = handler.CookieContainer.GetCookies(url);

            //handler.CookieContainer.GetCookies()
            url = new Uri("http://write.blog.csdn.net/postlist");
            Debug.Write(handler.CookieContainer.GetCookies(url).Count);
            str = await http.GetStringAsync(url);

            //content=new MultipartContent();
            //((MultipartContent)content)
            content = new MultipartFormDataContent();
            HandlCookie(handler,url);
            ((MultipartFormDataContent)content).Headers.Add("name", "file1");
            //((MultipartFormDataContent)content)
            ((MultipartFormDataContent)content).Headers.Add("filename", "20170114120751.png");
            var stream = new StreamContent(await File.OpenStreamForReadAsync());
            ((MultipartFormDataContent)content).Add(stream);
            str = await ((MultipartFormDataContent)content).ReadAsStringAsync();
            //http://write.blog.csdn.net/article/UploadImgMarkdown?parenthost=write.blog.csdn.net
            url = new Uri("http://write.blog.csdn.net/article/UploadImgMarkdown?parenthost=write.blog.csdn.net");
            HandlCookie(handler, url);

            var message = await http.PostAsync(url, content);
            if (message.StatusCode == HttpStatusCode.OK)
            {
                //str = await message.Content.ReadAsStringAsync();
                //message.Content.ReadAsStreamAsync()
                ResponseImage(message);
            }
        }

        public static CookieContainer AddCookieToContainer(string cookie, CookieContainer cc, string domain)
        {
            string[] tempCookies = cookie.Split(';');
            string tempCookie = null;
            int Equallength = 0;//  =的位置 
            string cookieKey = null;
            string cookieValue = null;
            //qg.gome.com.cn  cookie 
            for (int i = 0; i < tempCookies.Length; i++)
            {
                if (!string.IsNullOrEmpty(tempCookies[i]))
                {
                    tempCookie = tempCookies[i];

                    Equallength = tempCookie.IndexOf("=");

                    if (Equallength != -1)       //有可能cookie 无=，就直接一个cookiename；比如:a=3;ck;abc=; 
                    {
                        cookieKey = tempCookie.Substring(0, Equallength).Trim();
                        if (Equallength == tempCookie.Length - 1)    //这种是等号后面无值，如：abc=; 
                        {
                            cookieValue = "";
                        }
                        else
                        {
                            cookieValue = tempCookie.Substring(Equallength + 1, tempCookie.Length - Equallength - 1).Trim();
                        }
                    }

                    else
                    {
                        cookieKey = tempCookie.Trim();
                        cookieValue = "";
                    }

                    //cc.Add(new Cookie(cookieKey, cookieValue, "", domain));
                    cc.Add(new Uri(domain), new Cookie(cookieKey, cookieValue, "", domain));
                }

            }
            return cc;
        }

        private static void HandlCookie(HttpClientHandler handler, Uri url)
        {
            //uuid_tt_dd=-8518837881335297919_20170109; 
            //__message_sys_msg_id =0;
            //__message_gu_msg_id =0; 
            //__message_cnel_msg_id =0; 
            //__message_district_code =000000;
            //__message_in_school =0; _ga=GA1.2.1599396469.1483937637;
            //_message_m =3ndkuz0qmesws3ayo4leqnn0; 
            //UserName =lindexi_gd; 
            //UserInfo =sdHOjO9DCPtj%2BW%2B4g08kUGQFa9Bg1Y%2BlpMLzPyVuR2G1virxcRuehfjD7lBwL9uiOyel%2B4ruNHSaITmLws4DgBDj8Wy8XdIUjfOS98ZWlAB3C4vdJMgX5s92nGzCV19euO%2BfTlb9M488KfA%2BZ7TNVQ%3D%3D; 
            //UserNick =lindexi_gd; AU=747; UN=lindexi_gd; UE="lindexi_gd@163.com";
            //BT =1484964095225;
            //access -token=5f36bc49-0d17-41f1-8096-f5020892d887;
            //Hm_lvt_6bcd52f51e9b3dce32bec4a3997715ac =1484895363,1484962281,1484963306,1484967480;
            //Hm_lpvt_6bcd52f51e9b3dce32bec4a3997715ac =1484967480; 
            //dc_tos =ok409n;
            //dc_session_id =1484967490039
            foreach (Cookie temp in handler.CookieContainer.GetCookies(url))
            {
                string str = $"name={temp.Name};\ndomain={temp.Domain}\npath={temp.Path}\nvalue={temp.Value}\n\n";
                Debug.Write(str);
            }
            Debug.Write(handler.CookieContainer.GetCookies(url).Count);
            Debug.Write(handler.CookieContainer.GetCookieHeader(url));
            
            //name=UserName;
            //domain=.csdn.net
            //path=/
            //value=lindexi_gd

            //name=UserInfo;
            //domain=.csdn.net
            //path=/
            //value=sdHOjO9DCPtj%2BW%2B4g08kUGQFa9Bg1Y%2BlpMLzPyVuR2G1virxcRuehfjD7lBwL9uiOyel%2B4ruNHSaITmLws4DgBDj8Wy8XdIUjfOS98ZWlAB3C4vdJMgX5s92nGzCV19euO%2BfTlb9M488KfA%2BZ7TNVQ%3D%3D

            //name=UserNick;
            //domain=.csdn.net
            //path=/
            //value=lindexi_gd

            //name=AU;
            //domain=.csdn.net
            //path=/
            //value=747

            //name=UN;
            //domain=.csdn.net
            //path=/
            //value=lindexi_gd

            //name=BT;
            //domain=.csdn.net
            //path=/
            //value=1484968075530

            //name=access-token;
            //domain=.csdn.net
            //path=/
            //value=e97d57f0-e6a0-4ab7-98e2-ecf5afbfbfda

        }

        private async void ResponseImage(HttpResponseMessage message)
        {
            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    int length = 1024;
            //    byte[] buffer = new byte[length];
            //    using (GZipStream zip = new GZipStream(await message.Content.ReadAsStreamAsync(), CompressionLevel.Optimal))
            //    {
            //        int n;
            //        while ((n = zip.Read(buffer, 0, length)) > 0)
            //        {
            //            memoryStream.Write(buffer, 0, n);
            //        }
            //    }

            //    using (StreamReader stream = new StreamReader(memoryStream))
            //    {
            //        string str = stream.ReadToEnd();
            //    }
            //}

            using (StreamReader stream = new StreamReader(await message.Content.ReadAsStreamAsync()))
            {
                string str = stream.ReadToEnd();
            }
            //System.IO.Compression.DeflateStream

        }
    }
}
