using Microsoft.Extensions.Logging;

namespace LoggingPerformanceBenchmark
{
    public class LoggerRunner : RunnerBase
    {
        private readonly string _description;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger1;
        private readonly ILogger _logger2;
        
        public LoggerRunner(string description, ILoggerFactory loggerFactory, string category1Name, string category2Name)
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
                return "ILogger:" + _description;
            }
        }
        
        protected override void LogCritical1(int id, string message, params object[] data)
        {
            _logger1.LogCritical(id, message, data);
        }

        protected override void LogDebug1(int id, string message, params object[] data)
        {
            _logger1.LogDebug(id, message, data);
        }

        protected override void LogDebug2(int id, string message, params object[] data)
        {
            _logger2.LogDebug(id, message, data);
        }

        protected override void LogWarning2(int id, string message, params object[] data)
        {
            _logger2.LogWarning(id, message, data);
        }
    }
}
