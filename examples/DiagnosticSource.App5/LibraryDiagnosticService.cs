using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiagnosticSource.App5
{
    public class LibraryDiagnosticService : BackgroundService
    {
        private readonly ILogger<LibraryDiagnosticService> _logger;
        IDisposable? _keyValueSubscription;
        readonly object _keyValueSubscriptionLock = new object();
        private IDisposable? _listenerSubscription;

        public LibraryDiagnosticService(ILogger<LibraryDiagnosticService> logger)
        {
            _logger = logger;
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.LibraryDiagnosticServiceStarted(_logger, null);
            StartInstrumentation();
            return Task.CompletedTask;
        }
        
        private void StartInstrumentation()
        {
            _listenerSubscription = DiagnosticListener.AllListeners.Subscribe(
                new DiagnosticObserver(listener =>
                {
                    if (listener.Name == "DiagnosticSource.Library5")
                    {
                        lock (_keyValueSubscriptionLock)
                        {
                            if (_keyValueSubscription != null)
                            {
                                Log.KeyValueListenerReplaced(_logger, null);
                                _keyValueSubscription.Dispose();
                            }

                            _keyValueSubscription = listener.Subscribe(
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
