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
    public class RssCourier
    {
        private readonly ILogger<RssCourier> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RssCourier(ILogger<RssCourier> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public async void Start()
        {
            while (true)
            {
                _logger.LogInformation($"{DateTime.Now} 开始拉取博客");
                var minTime = TimeSpan.FromDays(2);

                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    await using var blogContext = serviceScope.ServiceProvider.GetService<BlogContext>();

                    await foreach (var blog in blogContext.Blog)
                    {
                        var blogDescriptionList = await GetBlog(blog.BlogRss);

                        foreach (var blogDescription in blogDescriptionList)
                        {
                            var distance = DateTime.Now - blogDescription.Time;

                            if (distance > minTime)
                            {
                                _logger.LogInformation($"{blogDescription.Title} 发布时间 {blogDescription.Time} 距离当前{distance.TotalDays:0}");
                                continue;
                            }

                            PostBlog(blogContext, blog.BlogRss, blogDescription);
                        }
                    }

                    blogContext.PublishedBlogList.RemoveRange(blogContext.PublishedBlogList.Where(publishedBlog => (DateTime.Now - publishedBlog.Time) > minTime));

                    blogContext.SaveChanges();
                }

                await Task.Delay(TimeSpan.FromMinutes(10));
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

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

                    if (blog.ServerUrl.Contains("qyapi.weixin"))
                    {
                        var qyweixin = new Qyweixin();
                        qyweixin.SendText(blog.ServerUrl, text);
                    }
                    else
                    {
                        var matterMost = new MatterMost();
                        matterMost.SendText(blog.ServerUrl, text);
                    }

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

        private static async Task<List<BlogDescription>> GetBlog(string url)
        {
            var task = Task.Run(async () =>
            {
                var blogList = new List<BlogDescription>();
                var newsFeedService = new NewsFeedService(url);
                var syndicationItems = await newsFeedService.GetNewsFeed();
                foreach (var syndicationItem in syndicationItems)
                {
                    var description =
                        syndicationItem.Description.Substring(0, Math.Min(200, syndicationItem.Description.Length));
                    var time = syndicationItem.Published;
                    var uri = syndicationItem.Links.FirstOrDefault()?.Uri;

                    if (time < syndicationItem.LastUpdated)
                    {
                        time = syndicationItem.LastUpdated;
                    }

                    blogList.Add(new BlogDescription()
                    {
                        Title = syndicationItem.Title,
                        Description = description,
                        Time = time.DateTime,
                        Url = uri?.AbsoluteUri
                    });
                }

                return blogList;
            });

            var t = Task.Delay(TimeSpan.FromMinutes(10));
            await Task.WhenAny(t, task);

            if (task.IsCompleted)
            {
                return await task;
            }

            return new List<BlogDescription>();
        }
    }
}