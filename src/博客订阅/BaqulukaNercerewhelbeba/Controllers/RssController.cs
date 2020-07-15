using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public IActionResult Post([FromBody] BlogRequest blogRequest)
        {
            if (!string.IsNullOrEmpty(blogRequest.MatterMostUrl))
            {
                if (blogRequest.BlogList != null)
                {
                    var blogList = new List<Blog>();

                    foreach (var blog in blogRequest.BlogList.Select(blog => new Blog()
                    {
                        ServerUrl = blogRequest.MatterMostUrl,
                        BlogRss = blog
                    }))
                    {
                        // 判断没有加入过
                        if (_blogContext.Blog.All(temp => temp.BlogRss != blog.BlogRss || temp.ServerUrl != blog.ServerUrl))
                        {
                            _blogContext.Blog.Add(blog);
                            blogList.Add(blog);
                        }
                    }

                    _blogContext.SaveChanges();

                    return Ok(new
                    {
                        Text = $"成功给{blogRequest.MatterMostUrl}添加{blogList.Count}博客",
                        BlogList = blogList,
                    });
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

        [HttpDelete]
        public IActionResult DeleteBlog([FromBody] BlogRequest blogRequest)
        {
            var str = new StringBuilder();
            str.AppendLine($"从 {blogRequest.MatterMostUrl} 移除");
            List<Blog> removeBlogList = new List<Blog>();
            foreach (var blog in blogRequest.BlogList)
            {
                removeBlogList.AddRange(_blogContext.Blog.Where(temp => temp.BlogRss == blog && temp.ServerUrl == blogRequest.MatterMostUrl));
            }

            _blogContext.Blog.RemoveRange(removeBlogList);
            _blogContext.SaveChanges();

            foreach (var blog in removeBlogList)
            {
                str.AppendLine(blog.BlogRss);
            }

            return Ok(str.ToString());
        }

        public IEnumerable<Blog> Get()
        {
            return _blogContext.Blog;
        }
    }


}