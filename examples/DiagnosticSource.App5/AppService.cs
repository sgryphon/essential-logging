using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DiagnosticSource.Library5;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiagnosticSource.App5
{
    public class AppService : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<AppService> _logger;

        public AppService(ILogger<AppService> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(0, stoppingToken);
            Log.AppServiceStarted(_logger, null);
            
            var primeGenerator1 = new PrimeGenerator();
            
            Console.WriteLine("Generating primes");
            var list1 = primeGenerator1.GeneratePrimes(5);
            Console.WriteLine(string.Join(",", list1));

            await Task.Delay(1000, stoppingToken);
            Console.WriteLine("Press Enter for next list");
            Console.ReadLine();
            
            Console.WriteLine("Generating primes");
            var list2 = primeGenerator1.GeneratePrimes(5);
            Console.WriteLine(string.Join(",", list2));

            _hostApplicationLifetime.StopApplication();
        }

    }
}
