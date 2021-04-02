using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiagnosticSource.App5
{
    /// <summary>
    ///     Consumes the diagnostic events from the library and logs them
    /// </summary>
    public class LibraryDiagnosticService : BackgroundService
    {
        private IDisposable? _keyValueSubscription;
        private readonly object _keyValueSubscriptionLock = new();
        private IDisposable? _listenerSubscription;
        private readonly ILogger<LibraryDiagnosticService> _logger;

        public LibraryDiagnosticService(ILogger<LibraryDiagnosticService> logger)
        {
            _logger = logger;
        }

        public override void Dispose()
        {
            _keyValueSubscription?.Dispose();
            _listenerSubscription?.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.LibraryDiagnosticServiceStarted(_logger, null);
            StartInstrumentation();
            return Task.CompletedTask;
        }

        private void StartInstrumentation()
        {
            _listenerSubscription = DiagnosticListener.AllListeners.Subscribe(listener =>
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

                        _keyValueSubscription = listener.Subscribe(keyValuePair =>
                        {
                            Log.DiagnosticReceived(_logger, keyValuePair.Key, keyValuePair.Value, null);
                        });
                    }
                }
            });
        }
    }
}
