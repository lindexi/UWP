using Microsoft.EntityFrameworkCore;
using OTAManager.Server.Model;

namespace OTAManager.Server.Data
{
    public class FileStorageContext : DbContext
    {
        public FileStorageContext(DbContextOptions<FileStorageContext> options)
            : base(options)
        {
        }

        public DbSet<FileStorageModel> FileStorageData { get; set; } = null!;
    }
}
