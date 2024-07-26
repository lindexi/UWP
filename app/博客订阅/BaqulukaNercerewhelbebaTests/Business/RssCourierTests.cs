using System;
using System.Text;
using BaqulukaNercerewhelbeba.Business;
using BaqulukaNercerewhelbeba.Data;
using BaqulukaNercerewhelbeba.Model;
using BaqulukaNercerewhelbeba.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MSTest.Extensions.Contracts;

namespace BaqulukaNercerewhelbeba.Business.Tests
{
    [TestClass()]
    public class RssCourierTests
    {
        [ContractTestCase]
        public void StartTest()
        {
            "多个微信地址和Mattermost地址，可以正确订阅".Test(async () =>
            {
                var mock = new Mock<INotifyProvider>();

                var builder = Host.CreateDefaultBuilder(Array.Empty<string>());
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<ITimeDelay>(s => new FakeTimeDelay());
                    services.AddTransient<IRssBlog>(s => new FakeRssBlog());
                    services.AddTransient<INotifyProvider>(s => mock.Object);

                    services.AddSingleton<RssCourier>();

                    services.AddDbContext<BlogContext>(options =>
                        options.UseInMemoryDatabase("Blog"));
                });

                var build = builder.Build();

                using (var serviceScope = build.Services.CreateScope())
                {
                    var blogContext = serviceScope.ServiceProvider.GetService<BlogContext>();
                    using (blogContext)
                    {
                        blogContext.Blog.AddRange(new[]
                        {
                            new Blog()
                            {
                                BlogRss = "1",
                                ServerUrl = "qiye"
                            },
                            new Blog()
                            {
                                BlogRss = "2",
                                ServerUrl = "qiye"
                            },
                            new Blog()
                            {
                                BlogRss = "2",
                                ServerUrl = "rss"
                            },
                        });

                        blogContext.SaveChanges();
                    }
                }

                var rssCourier = build.Services.GetService<RssCourier>();
                await rssCourier.Start();
            });
        }
    }
}
