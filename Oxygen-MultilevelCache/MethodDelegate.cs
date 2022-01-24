using System.Reflection;

namespace Oxygen.MulitlevelCache
{
    internal class MethodDelegate<Tobj, Tout, TaskTOut> : IMethodDelegate where Tout : class where TaskTOut : class
    {
        private IL1CacheServiceFactory L1;
        private IL2CacheServiceFactory L2;
        private Tobj Service;
        private readonly MethodInfo Method;
        private readonly bool IsTaskMethod = false;
        private readonly Func<Tobj, object?[]?, Tout> MethodFunCall;
        private string CacheKey { get; set; }
        private SystemCachedAttribute CachedAttr { get; set; }
        public MethodDelegate(MethodInfo method)
        {
            Method = method;
            IsTaskMethod = typeof(Tout) != typeof(TaskTOut);
            MethodFunCall = DelegateBuilder.CreateMethodDelegate<Tobj, Tout>(method);
            //获取方法缓存配置
            CachedAttr = Common.systemCachedAttrDir.ContainsKey(method) ? Common.systemCachedAttrDir[method] : default;
        }
        Tout? TryGetCache(MethodInfo methodInfo, object?[]? args)
        {
            //如果没有配置或者配置ttl设置为0
            if (CachedAttr == null || CachedAttr.ExpireSecond == 0)
                return default;
            else
            {
                //获取缓存key
                CacheKey = CachedAttr.CachedType == SystemCachedType.MethodAndParams ? Common.GetCachedKey(methodInfo, args) : Common.GetCachedKey(methodInfo);
                //从L1同步读取缓存
                var cacheResult = L1.Get<TaskTOut>(CacheKey);
                if (cacheResult == null)
                {
                    //创建一个线程中断器
                    var reset = new AutoResetEvent(false);
                    //创建一个同步对象用于接收L2线程回调的数据
                    var realResult = new AsyncLocal<TaskTOut>();
                    //创建一个任务并设置取消token
                    var cancelSource = new CancellationTokenSource();
                    var task = Task.Run(()=>L2.GetAsync<TaskTOut>(CacheKey), cancelSource.Token);
                    //启动任务
                    task.ContinueWith(t =>
                    {
                        if (t.Exception == null)
                            //如果任务顺利回调，则写缓存并阻止中断器
                            realResult.Value = t.Result;
                        reset.Set();
                    });
                    //如果设置了超时等待，则等待超时后取消任务并阻止中断器
                    if (CachedAttr.TimeOutMillisecond > 0)
                        Task.Delay(CachedAttr.TimeOutMillisecond).ContinueWith(t => { 
                            reset.Set(); 
                            cancelSource.Cancel(); 
                        });
                    //中断器开始中断当前线程并监听阻止信号
                    reset.WaitOne();
                    cacheResult = realResult.Value;
                    if (cacheResult == null)
                        return default;
                    else
                    {
                        //覆写L1
                        L1.Set(CacheKey, cacheResult, CachedAttr.ExpireSecond);
                        return Task.FromResult(cacheResult) as Tout;
                    }
                }
                else
                    return Task.FromResult(cacheResult) as Tout;
            }
        }
        void SetCache(Tout cacheResult)
        {
            if (CachedAttr != null && CachedAttr.ExpireSecond > 0)
            {
                if (IsTaskMethod)
                {
                    (cacheResult as Task<TaskTOut>)?.ContinueWith(t =>
                    {
                        L1.Set(CacheKey, t.Result, CachedAttr.ExpireSecond);
                        L2.SetAsync(CacheKey, t.Result, CachedAttr.ExpireSecond);
                    });
                }
                else
                {
                    L1.Set(CacheKey, cacheResult, CachedAttr.ExpireSecond);
                    L2.SetAsync(CacheKey, cacheResult, CachedAttr.ExpireSecond);
                }
            }
        }
        public object? Excute(object?[]? args)
        {
            //注入需要的构造函数和需要调用的服务类型实例
            LoadCacheSerivice();
            //调用缓存
            var cacheResult = TryGetCache(Method, args);
            if (cacheResult == null)
            {
                var result = MethodFunCall(Service, args);
                if (result != null)
                {
                    SetCache(result);
                    return result;
                }
                else
                {
                    return IsTaskMethod ? Task.FromResult(default(TaskTOut)) : default(Tout);
                }
            }
            else
            {
                return IsTaskMethod ? cacheResult as Tout: Task.FromResult(cacheResult);
            }
        }
        void LoadCacheSerivice()
        {
            L1 = Common.GetService<IL1CacheServiceFactory>();
            L2 = Common.GetService<IL2CacheServiceFactory>();
            Service = Common.GetService<Tobj>();
        }
    }
}
