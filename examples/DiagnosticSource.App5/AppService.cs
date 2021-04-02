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

            var primeGenerator = new PrimeGenerator();

            var list1 = primeGenerator.GeneratePrimes(10);
            Console.WriteLine(string.Join(",", list1));
            
            // Start instrumentation after first run
            StartInstrumentation();

            var list2 = primeGenerator.GeneratePrimes(10);
            Console.WriteLine(string.Join(",", list2));

            _hostApplicationLifetime.StopApplication();
        }

        private void StartInstrumentation()
        {
            var keyValueSubscription = default(IDisposable);
            var keyValueSubscriptionLock = new object();
            var listenerSubscription = DiagnosticListener.AllListeners.Subscribe(
                new DiagnosticObserver(listener =>
                {
                    if (listener.Name == "DiagnosticSource.Library5")
                    {
                        lock (keyValueSubscriptionLock)
                        {
                            if (keyValueSubscription != null)
                            {
                                Log.KeyValueListenerReplaced(_logger, null);
                                keyValueSubscription.Dispose();
                            }

                            keyValueSubscription = listener.Subscribe(
                                new KeyValueObserver(keyValuePair =>
                                {
                                    Log.DiagnosticReceived(_logger, keyValuePair.Key, keyValuePair.Value, null);
                                })
                            );
                        }
                    }
                }));
        }
    }
}
