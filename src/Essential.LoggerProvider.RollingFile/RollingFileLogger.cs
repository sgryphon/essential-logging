using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Essential.Logging.RollingFile
{
    public class RollingFileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly RollingFileLoggerProcessor _loggerProcessor;
        private LogTemplate _logTemplate;
        private RollingFileLoggerOptions _options;

        internal RollingFileLogger(string categoryName, RollingFileLoggerProcessor loggerProcessor)
        {
            _categoryName = categoryName;
            _loggerProcessor = loggerProcessor;
        }

        internal RollingFileLoggerOptions Options
        {
            get => _options;
            set
            {
                _options = value;
                _logTemplate = new LogTemplate(_options.Template);
            }
        }

        internal IExternalScopeProvider ScopeProvider { get; set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return ScopeProvider?.Push(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return Options.IsEnabled;
        }


        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
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
            object[] scopes = null;
            if (Options.IncludeScopes && scopeProvider != null)
            {
                var scopeList = new List<object>();
                scopeProvider.ForEachScope((scope, localList) =>
                {
                    localList.Add(scope);
                }, scopeList);
                scopes = scopeList.ToArray();
            }

            var output = _logTemplate.Bind(
                _categoryName,
                logLevel,
                eventId,
                message,
                exception,
                scopes
            );

            _loggerProcessor.EnqueueMessage(output);
        }
    }
}
