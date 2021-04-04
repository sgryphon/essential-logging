using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventSource.App5
{
    /// <summary>
    ///     Consumes the diagnostic events from the library and logs them
    /// </summary>
    public class LibraryEventService : BackgroundService
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        private LibraryEventListener? _listener;

        public LibraryEventService(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<LibraryEventService>();
        }

        public override void Dispose()
        {
            _listener?.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.LibraryDiagnosticServiceStarted(_logger, null);
            StartInstrumentation();
            return Task.CompletedTask;
        }

        private void StartInstrumentation()
        {
            _listener = new LibraryEventListener(_loggerFactory.CreateLogger<LibraryEventListener>());
        }
    }
}
