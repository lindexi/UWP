using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaqulukaNercerewhelbeba.Model;
using BaqulukaNercerewhelbeba.Util;

namespace BaqulukaNercerewhelbeba.Business
{
    public class RssBlog : IRssBlog
    {
        public async Task<List<BlogDescription>> FetchBlog(string url)
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
        }
    }
}
