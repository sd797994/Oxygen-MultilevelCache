using AppSimple.CacheImpl;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using Microsoft.Extensions.DependencyInjection;
using Oxygen.MulitlevelCache;
var config = new ManualConfig()
          .WithOptions(ConfigOptions.DisableOptimizationsValidator)
          .AddValidator(JitOptimizationsValidator.DontFailOnError)
          .AddLogger(ConsoleLogger.Default)
          .AddColumnProvider(DefaultColumnProviders.Instance);
BenchmarkRunner.Run<TestContext>(config);
public class TestContext
{
    public IServiceProvider serviceProvider;
    [GlobalSetup]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IUserService, UserService>();
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
        scope.ServiceProvider.GetService<IUserService>().GetUserName(1);
    }

    [Benchmark]
    public async Task UseAsyncCache()
    {
        using var scope = serviceProvider.CreateScope();
        await scope.ServiceProvider.GetService<IUserService>().GetUserNameAsync(1);
    }
}
public interface IUserService
{
    string GetUserName(int id);
    Task<string> GetUserNameAsync(int id);
}
public class UserService : IUserService
{
    [SystemCached]
    public string GetUserName(int id)
    {
        Thread.Sleep(5);//模拟数据库请求
        return $"{id}_aaa";
    }

    [SystemCached]
    public async Task<string> GetUserNameAsync(int id)
    {
        await Task.Delay(5);//模拟数据库请求
        return $"{id}_aaa";
    }
}