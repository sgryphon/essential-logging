using System;
using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace EventSource.App5
{
    public class LibraryEventListener : EventListener
    {
        private readonly ILogger _logger;

        public LibraryEventListener(ILogger<LibraryEventListener> logger)
        {
            _logger = logger;
            Console.WriteLine("Logger set: {0}", _logger);
        }
        
        protected override void OnEventSourceCreated(System.Diagnostics.Tracing.EventSource eventSource)
        {
            // This is called during base constructor (before _logger is set), with existing sources
            if (eventSource.Name == "EventSource-Library-PrimeGenerator")
            {
                Console.WriteLine("Enabling source {0}, logger: {1}", eventSource.Name, _logger);
                EnableEvents(eventSource, EventLevel.Verbose);
            }
            else
            {
                Console.WriteLine("Ignoring source {0}, logger: {1}", eventSource.Name, _logger);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            using var scope = _logger.BeginScope("[Task {0}, Opcode {1}, Keywords {2}, Activity {3}]", eventData.Task,
                eventData.Opcode, eventData.Keywords, eventData.ActivityId);
            if (eventData.PayloadNames != null)
            {
                if (eventData.Message != null)
                {
                    _logger.Log(Map(eventData.Level), new EventId(eventData.EventId, eventData.EventName),
                        string.Format(eventData.Message,
                            eventData.PayloadNames.Select(x => $"{{{x}}}").ToArray<object?>()),
                        eventData.Payload!.ToArray());
                }
                else
                {
                    _logger.Log(Map(eventData.Level), new EventId(eventData.EventId, eventData.EventName),
                        string.Concat(eventData.PayloadNames.Select(x => $"[{x} {{{x}}}]")),
                        eventData.Payload!.ToArray());
                }
            }
            else
            {
                _logger.Log(Map(eventData.Level), new EventId(eventData.EventId, eventData.EventName),
                    eventData.Message);
            }
        }

        private LogLevel Map(EventLevel eventDataLevel)
        {
            switch (eventDataLevel)
            {
                case EventLevel.LogAlways: return LogLevel.Critical;
                case EventLevel.Critical: return LogLevel.Critical;
                case EventLevel.Error: return LogLevel.Error;
                case EventLevel.Warning: return LogLevel.Warning;
                case EventLevel.Informational: return LogLevel.Information;
                case EventLevel.Verbose: return LogLevel.Debug;
                default: return LogLevel.Trace;
            }
        }
    }
}
