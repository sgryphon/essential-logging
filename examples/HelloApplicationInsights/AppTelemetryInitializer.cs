using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Essential.Logging
{
    public class AppTelemetryInitializer : ITelemetryInitializer
    {
        private readonly string? _appRoleName;
        private readonly string? _appVersion;

        public AppTelemetryInitializer()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var entryAssemblyName = entryAssembly?.GetName();
            _appRoleName = entryAssemblyName?.Name;
            var versionAttribute = entryAssembly?.GetCustomAttributes(false)
                .OfType<AssemblyInformationalVersionAttribute>()
                .FirstOrDefault();
            _appVersion = versionAttribute?.InformationalVersion ?? entryAssemblyName?.Version?.ToString();
        }

        public void Initialize(ITelemetry telemetry)
        {
            // https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-map
            telemetry.Context.Cloud.RoleName = _appRoleName;
            telemetry.Context.Component.Version = _appVersion;

            var user = Thread.CurrentPrincipal;
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                telemetry.Context.User.AuthenticatedUserId = user.Identity.Name;
            }
        }
    }
}
