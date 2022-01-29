using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Oxygen.MulitlevelCache
{
    internal class CachedHostedService : IHostedService
    {
        public CachedHostedService(IServiceProvider serviceProvider)
        {
            Common.SetServiceProvider(serviceProvider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
