using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Essential.LoggerProvider
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddRollingFile(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor
                .Singleton<ILoggerProvider, RollingFileLoggerProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor
                .Singleton<IConfigureOptions<RollingFileLoggerOptions>, RollingFileOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor
                .Singleton<IOptionsChangeTokenSource<RollingFileLoggerOptions>, LoggerProviderOptionsChangeTokenSource<
                    RollingFileLoggerOptions, RollingFileLoggerProvider>>());
            return builder;
        }

        public static ILoggingBuilder AddRollingFile(this ILoggingBuilder builder,
            Action<RollingFileLoggerOptions> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            builder.AddRollingFile();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
