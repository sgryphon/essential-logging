using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ActivitySource.App5
{
    internal static class Log
    {
        public static readonly Action<ILogger, Exception?> AppServiceStarted =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(1000, nameof(AppServiceStarted)),
                "App service started");

        public static readonly Action<ILogger, Exception?> LibraryActivityServiceStarted =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(1900, nameof(LibraryActivityServiceStarted)),
                "Library activity service started");
        
        public static readonly Action<ILogger, string, DateTimeOffset, IEnumerable<KeyValuePair<string, object?>>, Exception?> DiagnosticEvent =
            LoggerMessage.Define<string, DateTimeOffset, IEnumerable<KeyValuePair<string, object?>>>(LogLevel.Debug,
                new EventId(2900, nameof(DiagnosticEvent)),
                "Activity Event {Name} {Timestamp:s} : {Tags}");

        public static readonly Action<ILogger, string, IEnumerable<KeyValuePair<string, object?>>, Exception?> DiagnosticStart =
            LoggerMessage.Define<string, IEnumerable<KeyValuePair<string, object?>>>(LogLevel.Information,
                new EventId(2901, nameof(DiagnosticStart)),
                "Start Activity {OperationName} : {Tags}");
        
        public static readonly Action<ILogger, string, TimeSpan, IEnumerable<KeyValuePair<string, object?>>, Exception?> DiagnosticStop =
            LoggerMessage.Define<string, TimeSpan, IEnumerable<KeyValuePair<string, object?>>>(LogLevel.Information,
                new EventId(2902, nameof(DiagnosticStop)),
                "Stop Activity {OperationName}, duration {Duration} : {Tags}");

        public static readonly Action<ILogger, Exception?> ApplicationStopping =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(8000, nameof(ApplicationStopping)),
                "Application stopping");
        
    }
}
