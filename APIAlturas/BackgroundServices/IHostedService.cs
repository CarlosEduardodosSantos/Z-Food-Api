using System.Threading;
using System.Threading.Tasks;

namespace APIAlturas.BackgroundServices
{
    public interface IHostedService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}