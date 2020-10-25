using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaqulukaNercerewhelbeba.Data;
using BaqulukaNercerewhelbeba.Model;
using BaqulukaNercerewhelbeba.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BaqulukaNercerewhelbeba.Business
{
    /// <summary>
    /// 将Rss博客发送各个MatterMost链接
    /// </summary>
    public class RssCourier : IJob
    {
        private readonly ILogger<RssCourier> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RssCourier(ILogger<RssCourier> logger, IServiceProvider serviceProvider, IRssBlog rssBlog, INotifyProvider notifyProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _rssBlog = rssBlog;
            _notifyProvider = notifyProvider;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public async Task Start()
        {
            _logger.LogInformation($"{DateTime.Now} 开始拉取博客");
            var minTime = TimeSpan.FromDays(2);

            using (var serviceScope = _serviceProvider.CreateScope())
            {
                _logger.LogInformation($"开始读取数据库");
                await using var blogContext = serviceScope.ServiceProvider.GetService<BlogContext>();
                _logger.LogInformation($"开始遍历数据库");
                await foreach (var blog in blogContext.Blog)
                {
                    var blogDescriptionList = await GetBlog(blog.BlogRss);

                    foreach (var blogDescription in blogDescriptionList)
                    {
                        var distance = DateTime.Now - blogDescription.Time;

                        if (distance > minTime)
                        {
                            _logger.LogInformation(
                                $"{blogDescription.Title} 发布时间 {blogDescription.Time} 距离当前{distance.TotalDays:0}");
                            continue;
                        }

                        PostBlog(blogContext, blog.BlogRss, blogDescription);
                    }
                }

                // [客户端与服务器评估 - EF Core](https://docs.microsoft.com/zh-cn/ef/core/querying/client-eval )
                var dateTime = DateTime.Now.AddDays(-1 * minTime.TotalDays);

                blogContext.PublishedBlogList.RemoveRange(
                    blogContext.PublishedBlogList.Where(publishedBlog =>
                        dateTime > publishedBlog.Time));
                _logger.LogInformation($"开始保存数据库");
                blogContext.SaveChanges();
            }

            _logger.LogInformation($"{DateTime.Now} 拉取博客完成");
        }

        private readonly INotifyProvider _notifyProvider;

        private void PostBlog(BlogContext blogContext, string blogRss, BlogDescription blogDescription)
        {
            foreach (var blog in blogContext.Blog.Where(temp => temp.BlogRss == blogRss))
            {
                var publishedBlog = blogContext.PublishedBlogList
                    .FirstOrDefault(temp =>
                    temp.Blog == blogDescription.Url && temp.MatterMost == blog.ServerUrl);

                // 最近没有发布给这个mattermost这个博客
                if (publishedBlog is null)
                {
                    _logger.LogInformation($"给{blog.ServerUrl}发送{blogDescription.Title}博客");

                    var text = $"[{blogDescription.Title}]({blogDescription.Url})";

                    _notifyProvider.SendText(blog.ServerUrl, text);

                    _logger.LogInformation($"发布 {blogDescription.Title}");

                    blogContext.PublishedBlogList.Add(new PublishedBlog()
                    {
                        Blog = blogDescription.Url,
                        MatterMost = blog.ServerUrl,
                        Time = DateTime.Now,
                    });

                    blogContext.SaveChanges();
                }
                else
                {
                    _logger.LogInformation($"{blogDescription.Title}在 {publishedBlog.Time} 最近{blog.ServerUrl}发布过");
                }
            }
        }

        private async Task<List<BlogDescription>> GetBlog(string url)
        {
            var task = Task.Run(async () => await FetchBlog(url));

            var t = Task.Delay(TimeSpan.FromMinutes(10));
            await Task.WhenAny(t, task);

            if (task.IsCompleted)
            {
                return await task;
            }

            return new List<BlogDescription>();
        }

        private async Task<List<BlogDescription>> FetchBlog(string url)
        {
            return await _rssBlog.FetchBlog(url);
        }

        private readonly IRssBlog _rssBlog;
    }
}
