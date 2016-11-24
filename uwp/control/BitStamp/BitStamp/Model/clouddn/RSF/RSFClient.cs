// lindexi
// 16:34

using System.Collections.Generic;
using System.Threading.Tasks;
using lindexi.uwp.ImageShack.Model.Auth;
using lindexi.uwp.ImageShack.Model.RPC;
using Newtonsoft.Json;
using Qiniu.Conf;

namespace lindexi.uwp.ImageShack.Model.RSF
{
    /// <summary>
    ///     RS Fetch
    /// </summary>
    public class RSFClient : QiniuAuthClient
    {
        /// <summary>
        ///     RS Fetch Client
        /// </summary>
        /// <param name="bucketName">七牛云存储空间名称</param>
        public RSFClient(string bucketName)
        {
            this._bucketName = bucketName;
        }

        private const int MaxLimit = 1000;
        //失败重试次数
        private const int RetryTime = 3;

        /// <summary>
        ///     bucket name
        /// </summary>
        public string BucketName
        {
            get;
            private set;
        }

        /// <summary>
        ///     Fetch返回结果条目数量限制
        /// </summary>
        public int Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                _limit = value > MaxLimit ? MaxLimit : value;
            }
        }

        /// <summary>
        ///     文件前缀
        /// </summary>
        /// <value>
        ///     The prefix.
        /// </value>
        public string Prefix
        {
            set;
            get;
        }

        /// <summary>
        ///     Fetch 定位符.
        /// </summary>
        public string Marker
        {
            set;
            get;
        }

        /// <summary>
        ///     The origin Fetch interface,we recomment to use Next().
        /// </summary>
        /// <returns>
        ///     Dump
        /// </returns>
        /// <param name='bucket'>
        ///     Bucket name.
        /// </param>
        /// <param name='prefix'>
        ///     Prefix.
        /// </param>
        /// <param name='markerIn'>
        ///     Marker in.
        /// </param>
        /// <param name='limitFetch'>
        ///     Limit.
        /// </param>
        public async Task<DumpRet> ListPrefix(string bucket,
            string prefix = "",
            string markerIn = "",
            int limitFetch = 0)
        {
            string url = Config.RSF_HOST +
                         string.Format("/list?bucket={0}", bucket); // + bucketName + 
            if (!string.IsNullOrEmpty(markerIn))
            {
                url += string.Format("&marker={0}", markerIn);
            }
            if (!string.IsNullOrEmpty(prefix))
            {
                url += string.Format("&prefix={0}", prefix);
            }
            if (limitFetch > 0)
            {
                url += string.Format("&limit={0}", limitFetch);
            }
            for (int i = 0; i < RetryTime; i++)
            {
                CallRet ret = await Call(url);
                if (ret.OK)
                {
                    return JsonConvert.DeserializeObject<DumpRet>(ret.Response);
                }
            }
            return null;
        }

        /// <summary>
        ///     call this func before invoke Next()
        /// </summary>
        public void Init()
        {
            _end = false;
            Marker = string.Empty;
        }

        /// <summary>
        ///     Next.
        ///     <example>
        ///         <code>
        ///  public static void List (string bucket)
        /// {
        ///      RSF.RSFClient rsf = new Qiniu.RSF.RSFClient(bucket);
        ///      rsf.Prefix = "test";
        ///      rsf.Limit = 100;
        ///      List<DumpItem>
        ///                 items;
        ///                 while ((items=rsf.Next())!=null)
        ///                 {
        ///                 //todo
        ///                 }
        ///                 }s
        ///  </code>
        ///     </example>
        /// </summary>
        public async Task<List<DumpItem>> Next()
        {
            if (_end)
            {
                return null;
            }
            DumpRet ret = await ListPrefix(_bucketName,
                Prefix,
                Marker,
                _limit);
            if (ret.Items.Count == 0)
            {
                _end = true;
                return null;
            }
            Marker = ret.Marker;
            if (Marker == null)
            {
                _end = true;
            }
            return ret.Items;
        }

        private string _bucketName;

        private bool _end = false;

        private int _limit;
    }
}