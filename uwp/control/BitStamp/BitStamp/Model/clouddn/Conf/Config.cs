// lindexi
// 16:34

using System;
using System.Text;
using Windows.System.Profile;

namespace Qiniu.Conf
{
    public class Config
    {
        /// <summary>
        ///     初始化七牛帐户、请求地址等信息，不应在客户端调用。
        /// </summary>
        public static void Init()
        {
            // TODO
            //if (System.Configuration.ConfigurationManager.AppSettings["USER_AGENT"] != null) 
            //{
            //    USER_AGENT = System.Configuration.ConfigurationManager.AppSettings["USER_AGENT"];
            //}
            //if (System.Configuration.ConfigurationManager.AppSettings["ACCESS_KEY"] != null)
            //{
            //    ACCESS_KEY = System.Configuration.ConfigurationManager.AppSettings["ACCESS_KEY"];   
            //}
            //if (System.Configuration.ConfigurationManager.AppSettings["SECRET_KEY"] != null)
            //{
            //    SECRET_KEY = System.Configuration.ConfigurationManager.AppSettings["SECRET_KEY"];  
            //}
            //if (System.Configuration.ConfigurationManager.AppSettings["RS_HOST"] != null)
            //{
            //    RS_HOST = System.Configuration.ConfigurationManager.AppSettings["RS_HOST"];  
            //}
            //if (System.Configuration.ConfigurationManager.AppSettings["UP_HOST"] != null)
            //{
            //    UP_HOST = System.Configuration.ConfigurationManager.AppSettings["UP_HOST"];   
            //}
            //if (System.Configuration.ConfigurationManager.AppSettings["RSF_HOST"] != null)
            //{
            //    RSF_HOST = System.Configuration.ConfigurationManager.AppSettings["RSF_HOST"];  
            //}
            //if (System.Configuration.ConfigurationManager.AppSettings["PREFETCH_HOST"] != null)
            //{
            //    PREFETCH_HOST = System.Configuration.ConfigurationManager.AppSettings["PREFETCH_HOST"];
            //}
        }

        private static string getUa()
        {
            string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = v & 0x000000000000FFFFL;
            string osVersion = $"{v1}.{v2}.{v3}.{v4}";

            return "QiniuCsharp/" + VERSION + " (" + osVersion + "; )";
        }

        public static string VERSION = "6.1.8";

        public static string USER_AGENT = getUa();

        /// <summary>
        ///     七牛SDK对所有的字节编码采用utf-8形式 .
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;

        #region 帐户信息

        /// <summary>
        ///     七牛提供的公钥，用于识别用户
        /// </summary>
        public static string ACCESS_KEY = "<Please apply your access key>";

        /// <summary>
        ///     七牛提供的秘钥，不要在客户端初始化该变量
        /// </summary>
        public static string SECRET_KEY = "<Dont send your secret key to anyone>";

        #endregion

        #region 七牛服务器地址

        /// <summary>
        ///     七牛资源管理服务器地址
        /// </summary>
        public static string RS_HOST = "https://rs.Qbox.me";

        /// <summary>
        ///     七牛资源上传服务器地址.
        /// </summary>
        public const string UP_HOST = "https://up.qiniu.com";

        /// <summary>
        ///     七牛资源列表服务器地址.
        /// </summary>
        public static string RSF_HOST = "https://rsf.Qbox.me";

        public static string PREFETCH_HOST = "https://iovip.qbox.me";

        public static string API_HOST = "https://api.qiniu.com";

        #endregion
    }
}
