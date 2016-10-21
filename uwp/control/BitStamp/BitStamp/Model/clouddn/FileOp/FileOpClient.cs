using System;
using System.Net;
using System.IO;
using Qiniu.RPC;
using System.Diagnostics;

namespace Qiniu.FileOp
{
	static class FileOpClient
	{
		public static async System.Threading.Tasks.Task<CallRet> Get(string url)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
				request.Headers["User-Agent"] = Conf.Config.USER_AGENT;
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
			using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
				string responseStr = reader.ReadToEnd ();
				return new CallRet (statusCode, responseStr);
			}
		}
	}
}
