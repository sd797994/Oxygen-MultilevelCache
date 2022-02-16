using AppSimple.CacheImpl;
using AppSimple.MockService;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using Infrastructure.EfDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oxygen.MulitlevelCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSimple
{
    public class BenchmarkTest
    {
        public static void Run()
        {
            var config = new ManualConfig()
                      .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                      .AddValidator(JitOptimizationsValidator.DontFailOnError)
                      .AddLogger(ConsoleLogger.Default)
                      .AddColumnProvider(DefaultColumnProviders.Instance);
            BenchmarkRunner.Run<TestContext>(config);
        }
        public static async Task Debug()
        {
            var test = new TestContext();
            test.Setup();
            test.UseCache();
        }
    }
    public class TestContext
    {
        public IServiceProvider serviceProvider;
        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IArticleService, ArticleService>();
            serviceCollection.AddMemoryCache();
            serviceCollection.InjectionCached<L1Cache, L2Cache>();
            serviceCollection.AddDbContext<EfDbContext>(options => options.UseNpgsql("User ID=postgres;Password=Mytestpwd#123;Host=192.168.1.253;Port=32508;Database=UserDb;Pooling=true;"));
            serviceProvider = serviceCollection.BuildServiceProvider();
            Common.SetServiceProvider(serviceProvider);
            _ = cSRedisClientImpl.cSRedisClient.Value;
        }
        [Benchmark]
        public void DirectCallWithNoCache()
        {
            using var scope = serviceProvider.CreateScope();
            scope.ServiceProvider.GetService<IArticleService>().GetAllArticle();
        }

        [Benchmark]
        public async Task AsyncDirectCallWithNoCache()
        {
            using var scope = serviceProvider.CreateScope();
            await scope.ServiceProvider.GetService<IArticleService>().GetAllArticleAsync();
        }
        public void UseCache()
        {
            using var scope = serviceProvider.CreateScope();
            var article = scope.ServiceProvider.GetService<IArticleService>().GetArticleById(new Article() { Id = 1 });
        }
    }
}
