using System.Linq;
using System.Threading.Tasks;

namespace BaqulukaNercerewhelbeba.Model
{
    public class Blog
    {
        public int Id { set; get; }

        /// <summary>
        /// 发送到哪个地址
        /// </summary>
        public string ServerUrl { set; get; }

        /// <summary>
        /// 博客订阅链接
        /// </summary>
        public string BlogRss { set; get; }
    }
}
