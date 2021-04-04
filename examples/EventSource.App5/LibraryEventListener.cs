using System.Diagnostics.Tracing;
using Microsoft.Extensions.Logging;

namespace EventSource.App5
{
    public class LibraryEventListener : EventListener
    {
        private readonly ILogger _logger;

        public LibraryEventListener(ILogger<LibraryEventListener> logger)
        {
            _logger = logger;
        }
        
        protected override void OnEventSourceCreated(System.Diagnostics.Tracing.EventSource eventSource)
        {
            if (eventSource.Name == "EventSource-Library-PrimeGenerator")
            {
                EnableEvents(eventSource, EventLevel.Verbose);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            _logger.Log(Map(eventData.Level), new EventId(eventData.EventId, eventData.EventName),
                "Task {0}, Opcode {1}, Keywords {2}, Activity {3}, Message {4], PayloadNames {5}, Payload {6}",
                eventData.Task, eventData.Opcode, eventData.Keywords, eventData.ActivityId,
                eventData.Message, eventData.PayloadNames, eventData.Payload);
        }

        private LogLevel Map(EventLevel eventDataLevel)
        {
            switch (eventDataLevel)
            {
                case EventLevel.LogAlways: return LogLevel.Critical;
                case EventLevel.Critical: return LogLevel.Critical;
                case EventLevel.Error: return LogLevel.Critical;
                case EventLevel.Warning: return LogLevel.Critical;
                case EventLevel.Informational: return LogLevel.Critical;
                case EventLevel.Verbose: return LogLevel.Debug;
                default: return LogLevel.Trace;
            }
        }
    }
}
