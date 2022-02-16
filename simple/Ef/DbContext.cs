using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.EfDataAccess
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
            Console.WriteLine($"ef上下文被创建，上下文ID：{this.ContextId}");
        }
        public override void Dispose()
        {
            Console.WriteLine($"ef上下文被销毁，上下文ID：{this.ContextId}");
            base.Dispose();
        }
        public override async ValueTask DisposeAsync()
        {
            Console.WriteLine($"ef上下文被销毁，上下文ID：{this.ContextId}");
            await base.DisposeAsync();
        }
        public DbSet<Article> Article { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //启用Guid主键类型扩展
            modelBuilder.HasPostgresExtension("uuid-ossp");
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnModelCreating(modelBuilder);
        }
    }
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
