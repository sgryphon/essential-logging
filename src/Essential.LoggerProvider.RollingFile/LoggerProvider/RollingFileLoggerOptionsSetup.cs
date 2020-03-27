using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Essential.LoggerProvider
{
    internal class RollingFileLoggerOptionsSetup : ConfigureFromConfigurationOptions<RollingFileLoggerOptions>
    {
        public RollingFileLoggerOptionsSetup(ILoggerProviderConfiguration<RollingFileLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration)
        {
        }
    }
}
