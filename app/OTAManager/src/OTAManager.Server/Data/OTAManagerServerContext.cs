using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OTAManager.Server.Controllers;
using OTAManager.Server.Model;

namespace OTAManager.Server.Data
{
    public class OTAManagerServerContext : DbContext
    {
        public OTAManagerServerContext(DbContextOptions<OTAManagerServerContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 最新的版本信息
        /// </summary>
        public DbSet<ApplicationUpdateInfoModel> LatestApplicationUpdateInfo { get; set; }

        // 这里还需要一个最新的差分信息表
        // 一个全部版本的表
    }
}
