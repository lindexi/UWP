using System.Collections.Generic;
using System.Threading.Tasks;
using BaqulukaNercerewhelbeba.Model;

namespace BaqulukaNercerewhelbeba.Business
{
    public interface IRssBlog
    {
        Task<List<BlogDescription>> FetchBlog(string url);
    }
}
