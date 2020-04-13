using System;
using Microsoft.Extensions.Logging;

namespace HelloWebApp31
{
    public static class Log
    {
        public static readonly Action<ILogger, Exception?> UptimeServiceStarted =
            LoggerMessage.Define(LogLevel.Information,
                new EventId(1000, nameof(UptimeServiceStarted)),
                "Uptime service started.");

        public static readonly Action<ILogger, Guid, Exception?> GetIndex =
            LoggerMessage.Define<Guid>(LogLevel.Information,
                new EventId(2000, nameof(GetIndex)),
                "Get index ID {Id}.");

        public static readonly Action<ILogger, TimeSpan, Exception?> GetUptimeResult =
            LoggerMessage.Define<TimeSpan>(LogLevel.Debug,
                new EventId(6000, nameof(GetUptimeResult)),
                "GetUptime called, returning {Uptime}.");
    }
}
