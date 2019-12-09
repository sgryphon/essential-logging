using System;
using Microsoft.Extensions.Logging;

namespace LoggingPerformanceBenchmark
{
    public class LoggerMessageRunner : RunnerBase
    {
        private static readonly Action<ILogger, Exception> _logCritical9101 = LoggerMessage.Define(
            LogLevel.Critical, 
            new EventId(9101, nameof(LogCritical9101)), 
            "Message (critical error).");
        private static readonly Action<ILogger, Exception> _logCritical9102 = LoggerMessage.Define(
            LogLevel.Critical, 
            new EventId(9102, nameof(LogCritical9102)), 
            "Message (critical error).");
        
        private static readonly Action<ILogger, int, Exception> _logWarning4201 = LoggerMessage.Define<int>(
            LogLevel.Warning, 
            new EventId(4201, nameof(LogWarning4201)), 
            "Message (warning {Counter}).");
        private static readonly Action<ILogger, int, Exception> _logWarning4202 = LoggerMessage.Define<int>(
            LogLevel.Warning, 
            new EventId(4202, nameof(LogWarning4202)), 
            "Message (warning {Counter}).");
        private static readonly Action<ILogger, int, Exception> _logWarning4203 = LoggerMessage.Define<int>(
            LogLevel.Warning, 
            new EventId(4203, nameof(LogWarning4203)), 
            "Message (warning {Counter}).");
        
        private static readonly Action<ILogger, int, string, Exception> _logDebug31001 = LoggerMessage.Define<int, string>(
            LogLevel.Debug, 
            new EventId(31001, nameof(LogDebug31001)), 
            "Message 1 is {Data1} + {Data2}.");
        private static readonly Action<ILogger, int, Exception> _logDebug31002 = LoggerMessage.Define<int>(
            LogLevel.Debug, 
            new EventId(31002, nameof(LogDebug31002)), 
            "Message 2 is {Data1}.");
        private static readonly Action<ILogger, int, string, Exception> _logDebug32003 = LoggerMessage.Define<int, string>(
            LogLevel.Debug, 
            new EventId(32003, nameof(LogDebug32003)), 
            "Message 1 is {Data1} + {Data2}.");
        private static readonly Action<ILogger, int, Exception> _logDebug32004 = LoggerMessage.Define<int>(
            LogLevel.Debug, 
            new EventId(32004, nameof(LogDebug32004)), 
            "Message 2 is {Data1}.");

        private readonly string _description;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger1;
        private readonly ILogger _logger2;
        
        public LoggerMessageRunner(string description, ILoggerFactory loggerFactory, string category1Name, string category2Name)
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

        protected override void LogCritical9101(int id, string message)
        {
            _logCritical9101(_logger1, null);
        }

        protected override void LogCritical9102(int id, string message)
        {
            _logCritical9102(_logger1, null);
        }

        protected override void LogWarning4201(int id, string message, int counter2)
        {
            _logWarning4201(_logger2, counter2, null);
        }

        protected override void LogWarning4202(int id, string message, int counter2)
        {
            _logWarning4202(_logger2, counter2, null);
        }

        protected override void LogWarning4203(int id, string message, int counter2)
        {
            _logWarning4203(_logger2, counter2, null);
        }

        protected override void LogDebug31001(int id, string message, int data1, string data2)
        {
            _logDebug31001(_logger1, data1, data2, null);
        }

        protected override void LogDebug31002(int id, string message, int data1)
        {
            _logDebug31002(_logger1, data1, null);
        }

        protected override void LogDebug32003(int id, string message, int data1, string data2)
        {
            _logDebug32003(_logger2, data1, data2, null);
        }

        protected override void LogDebug32004(int id, string message, int data1)
        {
            _logDebug32004(_logger2, data1, null);
        }
    }
}
