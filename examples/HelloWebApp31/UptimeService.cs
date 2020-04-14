using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace HelloWebApp31
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
            _logger.LogInformation(2431,
                "Outer31 Id {ActId}, TraceId {ActTraceId}, SpanId {ActSpanId}, ParentId {ActParentId}, RootId {ActRootId}",
                Activity.Current?.Id, Activity.Current?.TraceId, Activity.Current?.SpanId, Activity.Current?.ParentId,
                Activity.Current?.RootId);
            var midActivity = new Activity("Mid");
            midActivity.Start();
            var innerActivity = new Activity("Inner");
            innerActivity.Start();
            _logger.LogInformation(2531,
                "Inner31 Id {ActId}, TraceId {ActTraceId}, SpanId {ActSpanId}, ParentId {ActParentId}, RootId {ActRootId}",
                Activity.Current?.Id, Activity.Current?.TraceId, Activity.Current?.SpanId, Activity.Current?.ParentId,
                Activity.Current?.RootId);
            midActivity.Stop();
            innerActivity.Stop();

            var uptime = DateTimeOffset.Now - _started;
            Log.GetUptimeResult(_logger, uptime, null);
            return uptime;
        }
        
    }
}
