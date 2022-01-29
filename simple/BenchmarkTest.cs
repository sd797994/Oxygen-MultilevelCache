using AppSimple.CacheImpl;
using AppSimple.MockService;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
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
            await test.UseAsyncCache();
        }
    }
    public class TestContext
    {
        public IServiceProvider serviceProvider;
        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IArticleService, ArticleService>();
            serviceCollection.AddMemoryCache();
            serviceCollection.InjectionCached<L1Cache, L2Cache>();
            serviceProvider = serviceCollection.BuildServiceProvider();
            Common.SetServiceProvider(serviceProvider);
            _ = cSRedisClientImpl.cSRedisClient.Value;
        }
        [Benchmark]
        public void DirectCallWithNoCache()
        {
            using var scope = serviceProvider.CreateScope();
            scope.ServiceProvider.GetService<UserService>().GetUserName(1);
        }

        [Benchmark]
        public async Task AsyncDirectCallWithNoCache()
        {
            using var scope = serviceProvider.CreateScope();
            await scope.ServiceProvider.GetService<UserService>().GetUserNameAsync(1);
        }
        [Benchmark]
        public void UseCache()
        {
            using var scope = serviceProvider.CreateScope();
            var name = scope.ServiceProvider.GetService<IUserService>().GetUserName(1);
            var article = scope.ServiceProvider.GetService<IArticleService>().GetUserArticleTitle(name);
        }

        [Benchmark]
        public async Task UseAsyncCache()
        {
            using var scope = serviceProvider.CreateScope();
            var name = await scope.ServiceProvider.GetService<IUserService>().GetUserNameAsync(1);
            var article =await scope.ServiceProvider.GetService<IArticleService>().GetUserArticleTitleAsync(name);
        }
    }
}
