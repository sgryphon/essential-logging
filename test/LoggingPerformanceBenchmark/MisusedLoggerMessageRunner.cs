using System;
using Microsoft.Extensions.Logging;

namespace LoggingPerformanceBenchmark
{
    public class MisusedLoggerMessageRunner : RunnerBase
    {
        private static readonly Action<ILogger, Exception> _logCritical = LoggerMessage.Define(
            LogLevel.Critical, 
            new EventId(1, "LogCritical"), 
            "Message (critical error).");
        private static readonly Action<ILogger, int, Exception> _logWarning = LoggerMessage.Define<int>(
            LogLevel.Warning, 
            new EventId(2, "LogWarning"), 
            "Message (warning {Counter}).");
        private static readonly Action<ILogger, int, string, Exception> _logDebugA = LoggerMessage.Define<int, string>(
            LogLevel.Debug, 
            new EventId(2, "LogWarning"), 
            "Message 1 is {Data1} + {Data2}.");
        private static readonly Action<ILogger, int, Exception> _logDebugB = LoggerMessage.Define<int>(
            LogLevel.Debug, 
            new EventId(2, "LogWarning"), 
            "Message 2 is {Data1}.");

        private readonly string _description;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger1;
        private readonly ILogger _logger2;
        
        public MisusedLoggerMessageRunner(string description, ILoggerFactory loggerFactory, string category1Name, string category2Name)
        {
            _description = description;
            _loggerFactory = loggerFactory;
            _logger1 = loggerFactory.CreateLogger(category1Name);
            _logger2 = loggerFactory.CreateLogger(category2Name);
        }
        
        public override string Name
        {
            get
            {
                return "LoggerMessage:" + _description;
            }
        }
        
        protected override void LogCritical1(int id, string message, params object[] data)
        {
            _logCritical(_logger1, null);
        }

        protected override void LogDebug1(int id, string message, params object[] data)
        {
            if (data.Length == 2)
            {
                _logDebugA(_logger1, (int)data[0], (string)data[1], null);
            }
            else
            {
                _logDebugB(_logger1, (int)data[0], null);
            }
        }

        protected override void LogDebug2(int id, string message, params object[] data)
        {
            if (data.Length == 2)
            {
                _logDebugA(_logger2, (int)data[0], (string)data[1], null);
            }
            else
            {
                _logDebugB(_logger2, (int)data[0], null);
            }
        }

        protected override void LogWarning2(int id, string message, params object[] data)
        {
            _logWarning(_logger2, (int)data[0], null);
        }
    }
}
