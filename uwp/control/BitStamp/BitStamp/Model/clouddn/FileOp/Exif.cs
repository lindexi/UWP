using System;
using Qiniu.RPC;

namespace Qiniu.FileOp
{
	public static class Exif
	{
		public static string MakeRequest (string url)
		{
			return url + "?exif";
		}

		public static async System.Threading.Tasks.Task<ExifRet> Call(string url)
		{
			CallRet callRet = await FileOpClient.Get(url);
			return new ExifRet(callRet);
		}
	}
}
