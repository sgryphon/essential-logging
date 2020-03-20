using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Essential.LoggerProvider
{
    [ProviderAlias("RollingFile")]
    public class RollingFileLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly ConcurrentDictionary<string, RollingFileLogger> _loggers;
        private readonly IOptionsMonitor<RollingFileLoggerOptions> _options;

        private readonly IDisposable _optionsReloadToken;
        private readonly RollingFileLoggerProcessor _processor;
        private IExternalScopeProvider _scopeProvider = default!;

        public RollingFileLoggerProvider(IOptionsMonitor<RollingFileLoggerOptions> options)
        {
            _options = options;
            _processor = new RollingFileLoggerProcessor();
            _loggers = new ConcurrentDictionary<string, RollingFileLogger>();
            ReloadLoggerOptions(options.CurrentValue);
            _optionsReloadToken = _options.OnChange(ReloadLoggerOptions);
        }

        public ILogger CreateLogger(string name)
        {
            return _loggers.GetOrAdd(name,
                loggerName =>
                    new RollingFileLogger(name, _processor)
                    {
                        Options = _options.CurrentValue, 
                        ScopeProvider = _scopeProvider
                    });
        }

        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
            _processor.Dispose();
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
            foreach (var logger in _loggers)
            {
                logger.Value.ScopeProvider = scopeProvider;
            }
        }

        private void ReloadLoggerOptions(RollingFileLoggerOptions options)
        {
            _processor.Options = options;
            foreach (var logger in _loggers)
            {
                logger.Value.Options = options;
            }
        }
    }
}
