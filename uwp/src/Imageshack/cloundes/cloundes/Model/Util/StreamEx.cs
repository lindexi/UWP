// lindexi
// 16:34

using System.IO;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.Conf;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.Util
{
    public static class StreamEx
    {
        /// <summary>
        ///     string To Stream
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Stream ToStream(string str)
        {
            Stream s = new MemoryStream(Config.Encoding.GetBytes(str));
            return s;
        }
    }
}
