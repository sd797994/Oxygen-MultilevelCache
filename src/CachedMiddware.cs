using Microsoft.AspNetCore.Http;

namespace Oxygen.MulitlevelCache
{
    internal class CachedMiddware
    {
        private readonly RequestDelegate _next;

        public CachedMiddware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Common.ServiceProvider.Value = context.RequestServices;
            await _next.Invoke(context);
        }
    }
}
