using System;
using Qiniu.RPC;

namespace Qiniu.FileOp
{
	public static class ImageInfo
	{
		public static string MakeRequest (string url)
		{
			return url + "?imageInfo";
		}

		public static async System.Threading.Tasks.Task<ImageInfoRet> Call(string url)
		{
			CallRet callRet = await FileOpClient.Get(url);
			return new ImageInfoRet(callRet);
		}
	}
}
