// lindexi
// 16:34

using System.Net;

namespace lindexi.uwp.ImageShack.Model.Util
{
    /// <summary>
    ///     String辅助函数
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        ///     对字符串进行Url编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUrlEncode(string value)
        {
            return WebUtility.UrlEncode(value);
        }
    }
}
