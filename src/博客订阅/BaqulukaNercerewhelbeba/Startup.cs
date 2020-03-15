using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BaqulukaNercerewhelbeba.Business;
using BaqulukaNercerewhelbeba.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BaqulukaNercerewhelbeba.Data;
using BaqulukaNercerewhelbeba.Util;

namespace BaqulukaNercerewhelbeba
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddTransient<ITimeDelay>(s => new TimeDelay());
            services.AddTransient<IRssBlog>(s => new RssBlog());
            services.AddTransient<INotifyProvider>(s => new NotifyProvider());
            services.AddTransient<TaskWork>();

            services.AddSingleton<RssCourier>();

            FindDb(services);
        }

        private void FindDb(IServiceCollection services)
        {
            // 如果配置里面有说到路径的话，使用配置的文件，如果没有尝试从环境变量获取
            const string sqliteFilePath = "SqliteFilePath";

            var filePath = Configuration[sqliteFilePath];
            if (!string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine($"Read {filePath} from config");
                SetSqlite(services, filePath);
                return;
            }

            // 尝试从环境变量获取
            filePath = Environment.GetEnvironmentVariable(sqliteFilePath);
            if (!string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine($"Read {filePath} from Environment value");
                SetSqlite(services, filePath);
                return;
            }

            Console.WriteLine($"Can not find the db file path, will use memory db");
            services.AddDbContext<BlogContext>(options =>
                options.UseInMemoryDatabase("Blog"));
        }

        private void SetSqlite(IServiceCollection services, string filePath)
        {
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(filePath)));

                var emptyDb = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "blog.db");
                File.Copy(emptyDb, filePath);
            }

            services.AddDbContext<BlogContext>(option => option.UseSqlite($"Filename={filePath}"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
