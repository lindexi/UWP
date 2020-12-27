using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OTAManager.Server.Controllers;

namespace OTAManager.Server.Data
{
    public class OTAManagerServerContext : DbContext
    {
        public OTAManagerServerContext (DbContextOptions<OTAManagerServerContext> options)
            : base(options)
        {
        }

        public DbSet<OTAManager.Server.Controllers.ApplicationUpdateInfo> LatestApplicationUpdateInfo { get; set; }
    }
}
