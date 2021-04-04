using System;
using Microsoft.Extensions.Logging;

namespace EventSource.App5
{
    internal static class Log
    {
        public static readonly Action<ILogger, Exception?> AppServiceStarted =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(1000, nameof(AppServiceStarted)),
                "App service started");

        public static readonly Action<ILogger, Exception?> StartListeningToLibrary =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(1901, nameof(StartListeningToLibrary)),
                "Listening to library event source");

        public static readonly Action<ILogger, string?, Exception?> IgnoringEventSource =
            LoggerMessage.Define<string?>(LogLevel.Debug,
                new EventId(1902, nameof(IgnoringEventSource)),
                "Ignoring event source {EventSourceName}");

        public static readonly Action<ILogger, Exception?> LibraryDiagnosticServiceStarted =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(1900, nameof(LibraryDiagnosticServiceStarted)),
                "Library diagnostic service started");
        
        public static readonly Action<ILogger, Exception?> ApplicationStopping =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(8000, nameof(ApplicationStopping)),
                "Application stopping");
        
    }
}
