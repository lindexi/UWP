using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BaqulukaNercerewhelbeba.Model;

namespace BaqulukaNercerewhelbeba.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext (DbContextOptions<BlogContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().HasIndex(temp => temp.BlogRss);

            modelBuilder.Entity<PublishedBlog>().HasIndex(temp => new
            {
                temp.Blog, temp.MatterMost
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BaqulukaNercerewhelbeba.Model.Blog> Blog { get; set; }

        public DbSet<PublishedBlog> PublishedBlogList { set; get; }
    }
}
