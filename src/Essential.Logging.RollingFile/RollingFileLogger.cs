using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Essential.Logging.RollingFile
{
    public class RollingFileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly RollingFileLoggerProcessor _loggerProcessor;
        TraceFormatter _traceFormatter = new TraceFormatter();

        internal RollingFileLogger(string categoryName, RollingFileLoggerProcessor loggerProcessor)
        {
            _categoryName = categoryName;
            _loggerProcessor = loggerProcessor;
        }
        
        internal IExternalScopeProvider ScopeProvider { get; set; }

        internal RollingFileLoggerOptions Options { get; set; }
        
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            var scopeProvider = ScopeProvider;
            object[] data = null;
            if (Options.IncludeScopes && scopeProvider != null)
            {
                var dataList = new List<object>();
                scopeProvider.ForEachScope((scope, localDataList) =>
                {
                    localDataList.Add(scope);
                }, dataList);
                if (dataList.Count > 0)
                {
                    data = dataList.ToArray();
                }
            }
            
            var output = _traceFormatter.Format(Options.Template,
                _categoryName,
                logLevel,
                eventId,
                message,
                data
            );
            
            _loggerProcessor.EnqueueMessage(output);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return Options.IsEnabled;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return ScopeProvider?.Push(state);
        }
    }
}
