using System.Reflection;

namespace Oxygen.MulitlevelCache
{
    internal class CachedProxy<Timpl> : DispatchProxy
    {
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            return DelegateBuilder.GetDelegate(targetMethod).Excute(args);
        }
    }
}
