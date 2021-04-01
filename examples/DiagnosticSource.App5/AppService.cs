using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DiagnosticSource.Library5;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiagnosticSource.App5
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public AppService(ILogger<AppService> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(0, stoppingToken);
            Log.AppServiceStarted(_logger, null);
            
            var primeGenerator = new PrimeGenerator();

            var list1 = primeGenerator.GeneratePrimes(10);
            Console.WriteLine(string.Join(",", list1));

            var list2 = primeGenerator.GeneratePrimes(10);
            Console.WriteLine(string.Join(",", list2));
            
            _hostApplicationLifetime.StopApplication();
        }
    }
}
