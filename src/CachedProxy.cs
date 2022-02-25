using System.Reflection;

namespace Oxygen.MulitlevelCache
{
    public class CachedProxy<Timpl> : DispatchProxy
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return DelegateBuilder.GetDelegate(targetMethod).Excute(args);
        }
    }
}
