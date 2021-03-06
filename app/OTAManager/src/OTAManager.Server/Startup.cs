﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OTAManager.Server.Controllers;
using OTAManager.Server.Core;
using OTAManager.Server.Data;

namespace OTAManager.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        private readonly IWebHostEnvironment _hostEnvironment;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileStorage, FileStorage>();

            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            services.AddDbContext<OTAManagerServerContext>(options =>
                    options.UseSqlite($"Data Source={_hostEnvironment.ContentRootPath}/OTAManager.db"));
            services.AddDbContext<FileStorageContext>(options =>
                options.UseSqlite($"Data Source={_hostEnvironment.ContentRootPath}/FileStorage.db"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
