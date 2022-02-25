using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public DbSet<Account> Account { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //启用Guid主键类型扩展
            modelBuilder.HasPostgresExtension("uuid-ossp");
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnModelCreating(modelBuilder);
        }
    }
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 账户状态
        /// </summary>
        public int State { get; set; }
    }
}
