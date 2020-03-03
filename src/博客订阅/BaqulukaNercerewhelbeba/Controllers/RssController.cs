using System.Collections.Generic;
using System.Linq;
using BaqulukaNercerewhelbeba.Data;
using BaqulukaNercerewhelbeba.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaqulukaNercerewhelbeba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RssController : ControllerBase
    {
        private readonly BlogContext _blogContext;

        /// <inheritdoc />
        public RssController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        [HttpPost]
        public IActionResult Post([FromBody]BlogRequest blogRequest)
        {
            if (!string.IsNullOrEmpty(blogRequest.MatterMostUrl))
            {
                if (blogRequest.BlogList != null)
                {
                    foreach (var blog in blogRequest.BlogList.Select(blog => new Blog()
                    {
                        ServerUrl = blogRequest.MatterMostUrl,
                        BlogRss = blog
                    }))
                    {
                        // 判断没有加入过
                        if (_blogContext.Blog.All(temp => temp.BlogRss != blog.BlogRss && temp.ServerUrl != blog.ServerUrl))
                        {
                            _blogContext.Blog.Add(blog);
                        }
                    }

                    _blogContext.SaveChanges();

                    return Ok($"成功给{blogRequest.MatterMostUrl}添加{blogRequest.BlogList.Count}博客");
                }
                else
                {
                    return BadRequest("订阅博客列表不能为空");
                }
            }
            else
            {
                return BadRequest("MatterMostUrl 不能为空");
            }
        }

        public IEnumerable<Blog> Get()
        {
            return _blogContext.Blog;
        }
    }


}