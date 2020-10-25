using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaqulukaNercerewhelbeba.Model;

namespace BaqulukaNercerewhelbeba.Business.Tests
{
    public class FakeRssBlog : IRssBlog
    {
        /// <inheritdoc />
        public Task<List<BlogDescription>> FetchBlog(string url)
        {
            var lastTime = _lastTime.AddDays(1);
            _lastTime = lastTime;

            return Task.FromResult(new List<BlogDescription>()
            {
                new BlogDescription()
                {
                    Description = "Description1",
                    Time = lastTime,
                    Title = $"Title1 {url}",
                    Url = "http://blog.lindexi.com/1" + url,
                },
                new BlogDescription()
                {
                    Description = "Description2",
                    Time = lastTime,
                    Title = $"Title2 {url}",
                    Url = "http://blog.lindexi.com/2" + url,
                },
            });
        }

        private string GetBlogUrl()
        {
            // 每次的博客都应该不重复，这里  + Count++ 的意思就是先加上 Count 的值，然后下一句话让 Count 自己加一
            return "http://blog.lindexi.com" + Count++;
        }

        private int Count { set; get; } = 1;

        private DateTime _lastTime = DateTime.Now;
    }
}
