﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaqulukaNercerewhelbeba.Business;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BaqulukaNercerewhelbeba
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Environment.CommandLine);

            var build = CreateHostBuilder(args).Build();

            var rssCourier = build.Services.GetService<RssCourier>();
            var taskWork = build.Services.GetService<TaskWork>();
            taskWork.StartWork(rssCourier);

            build.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://0.0.0.0:12367");
                });
    }

    /*
     * {
     *      "MatterMostUrl" : "http://127.0.0.1:8065/hooks/357owee7f7rumn316mcdrmmiko",
     *      "BlogList" : ["https://blog.lindexi.com/feed.xml"]
     * }
     */
}
