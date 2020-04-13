using System;
using Microsoft.Extensions.Logging;

namespace HelloWebApp22
{
    public class UptimeService
    {
        private readonly ILogger<UptimeService> _logger;
        private readonly DateTimeOffset _started;

        public UptimeService(ILogger<UptimeService> logger)
        {
            _logger = logger;
            _started = DateTimeOffset.Now;

            Log.UptimeServiceStarted(_logger, null);
        }

        public TimeSpan GetUptime()
        {
            var uptime = DateTimeOffset.Now - _started;
            Log.GetUptimeResult(_logger, uptime, null);
            return uptime;
        }
        
    }
}
