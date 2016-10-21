using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Qiniu.RPC
{
	public class Client
	{       
		public virtual void SetAuth (HttpWebRequest request, Stream body)
		{
		}

		public async System.Threading.Tasks.Task<CallRet> Call(string url)
		{
			Debug.WriteLine("Client.Post ==> URL: " + url);
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Headers["User-Agent"] = Conf.Config.USER_AGENT;
				request.Method = "POST";
				SetAuth(request, null);
				using (HttpWebResponse response = (await request.GetResponseAsync()) as HttpWebResponse)
				{
					return HandleResult(response);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
				return new CallRet(HttpStatusCode.BadRequest, e);
			}
		}

		public async System.Threading.Tasks.Task<CallRet> CallWithBinary(string url, string contentType, Stream body, long length)
		{
			Debug.WriteLine("Client.PostWithBinary ==> URL: {0} Length:{1}", url, length);
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Headers["User-Agent"] = Conf.Config.USER_AGENT;
				request.Method = "POST";
				request.ContentType = contentType;
				request.Headers["Content-Length"] = length.ToString();
				SetAuth(request, body);
				using (Stream requestStream = await request.GetRequestStreamAsync())
				{
					Util.IO.CopyN(requestStream, body, length);
				}

				using (HttpWebResponse response = (await request.GetResponseAsync()) as HttpWebResponse)
				{
					return HandleResult(response);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
				return new CallRet(HttpStatusCode.BadRequest, e);
			}
		}

		public static CallRet HandleResult (HttpWebResponse response)
		{
			HttpStatusCode statusCode = response.StatusCode;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseStr = reader.ReadToEnd();
                return new CallRet(statusCode, responseStr);
            }
		}
	}
}