using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Essential.LoggerProvider
{
    internal class RollingFileOptionsSetup : ConfigureFromConfigurationOptions<RollingFileLoggerOptions>
    {
        public RollingFileOptionsSetup(ILoggerProviderConfiguration<RollingFileLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration)
        {
        }
    }
}
