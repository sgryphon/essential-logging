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
    }
}
