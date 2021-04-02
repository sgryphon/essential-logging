using System;
using Microsoft.Extensions.Logging;

namespace DiagnosticSource.App5
{
    internal static class Log
    {
        public static readonly Action<ILogger, Exception?> AppServiceStarted =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(1000, nameof(AppServiceStarted)),
                "App service started");

        public static readonly Action<ILogger, Exception?> KeyValueListenerReplaced =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(1901, nameof(KeyValueListenerReplaced)),
                "New key value listener for DiagnosticSource.Library5 is replacing existing listener");

        public static readonly Action<ILogger, Exception?> LibraryDiagnosticServiceStarted =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(1900, nameof(LibraryDiagnosticServiceStarted)),
                "Library diagnostic service started");

        
        public static readonly Action<ILogger, string, object?, Exception?> DiagnosticReceived =
            LoggerMessage.Define<string, object?>(LogLevel.Debug,
                new EventId(2900, nameof(DiagnosticReceived)),
                "Diagnostic {Key}: {Value}");

        public static readonly Action<ILogger, string, object?, Exception?> DiagnosticStart =
            LoggerMessage.Define<string, object?>(LogLevel.Information,
                new EventId(2901, nameof(DiagnosticStart)),
                "Diagnostic Activity {OperationName} Start: {Value}");
        
        public static readonly Action<ILogger, string, TimeSpan, Exception?> DiagnosticStop =
            LoggerMessage.Define<string, TimeSpan>(LogLevel.Information,
                new EventId(2902, nameof(DiagnosticStop)),
                "Diagnostic Activity {OperationName} Stop, duration {Duration}");

        public static readonly Action<ILogger, Exception?> ApplicationStopping =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(8000, nameof(ApplicationStopping)),
                "Application stopping");
        
    }
}
