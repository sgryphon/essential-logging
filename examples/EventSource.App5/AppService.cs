using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EventSource.Library5;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventSource.App5
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
            try
            {
                await Task.Delay(0, stoppingToken);
                Log.AppServiceStarted(_logger, null);

                Console.WriteLine();
                Console.WriteLine(
                    "Generating primes, before diagnostics has had time to initialise, and no other listeners");
                var primeGenerator1 = new PrimeGenerator();
                _ = primeGenerator1.GeneratePrimes(50);
                var list1 = primeGenerator1.GeneratePrimes(3);
                Console.WriteLine(string.Join(",", list1));

                await Task.Delay(500, stoppingToken);
                Console.WriteLine();
                Console.WriteLine("Press Enter for next list");
                Console.ReadLine();

                await Task.Delay(500, stoppingToken);
                Console.WriteLine();
                Console.WriteLine("Generating primes");
                var list2 = primeGenerator1.GeneratePrimes(3);
                Console.WriteLine(string.Join(",", list2));
                _ = primeGenerator1.GeneratePrimes(0);

                await Task.Delay(500, stoppingToken);
                Console.WriteLine();
                Console.WriteLine("New generator");
                var primeGenerator2 = new PrimeGenerator();
                var list3 = primeGenerator2.GeneratePrimes(3);
                Console.WriteLine(string.Join(",", list3));

                await Task.Delay(500, stoppingToken);
                Console.WriteLine();
                Console.WriteLine("From original generator");
                var list4 = primeGenerator1.GeneratePrimes(3);
                Console.WriteLine(string.Join(",", list4));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }
            
            _hostApplicationLifetime.StopApplication();
        }
    }
}
