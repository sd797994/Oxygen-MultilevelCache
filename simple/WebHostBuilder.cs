using AppSimple.CacheImpl;
using AppSimple.MockService;
using Infrastructure.EfDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oxygen.MulitlevelCache;

namespace AppSimple
{
    public class WebHostBuilder
    {
        public static async Task Run()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddMemoryCache();
            builder.Services.InjectionCached<L1Cache, L2Cache>();
            builder.Services.AddDbContext<EfDbContext>(options => options.UseNpgsql("User ID=postgres;Password=Mytestpwd#123;Host=192.168.1.253;Port=30432;Database=AccountDb;Pooling=true;"));
            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseRouting();
            app.UseEndpoints(endpoints =>endpoints.MapControllers());   
            await app.RunAsync();
        }
    }
    [Route("/")]
    public class TestController: ControllerBase
    {
        private readonly IArticleService articleService;
        public TestController(IArticleService articleService)
        {
            this.articleService = articleService;
        }
        public async Task<Account> Index()
        {
            return await articleService.GetArticleByIdAsync(new Account() { Id = Guid.Empty });
        }
    }
}
