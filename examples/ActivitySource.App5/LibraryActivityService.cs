using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ActivitySource.App5
{
    /// <summary>
    ///     Consumes the diagnostic events from the library and logs them
    /// </summary>
    public class LibraryActivityService : BackgroundService
    {
        private ActivityListener? _activityListener;
        private readonly ILogger<LibraryActivityService> _logger;

        public LibraryActivityService(ILogger<LibraryActivityService> logger)
        {
            _logger = logger;
        }

        public override void Dispose()
        {
            _activityListener?.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.LibraryActivityServiceStarted(_logger, null);
            StartInstrumentation();
            return Task.CompletedTask;
        }

        private void StartInstrumentation()
        {
            _activityListener = new ActivityListener
            {
                ShouldListenTo = _ => true,
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
                ActivityStarted = activity =>
                {
                    // Loggers have special handling to render IEnumerable<KeyValuePair<string, object?>>
                    Log.DiagnosticStart(_logger, activity.OperationName, activity.TagObjects, null);
                },
                ActivityStopped = activity =>
                {
                    foreach (var activityEvent in activity.Events)
                    {
                        Log.DiagnosticEvent(_logger, activityEvent.Name, activityEvent.Timestamp, activityEvent.Tags,
                            null);
                    }

                    Log.DiagnosticStop(_logger, activity.OperationName, activity.Duration, activity.TagObjects, null);
                }
            };
            System.Diagnostics.ActivitySource.AddActivityListener(_activityListener);
        }
    }
}
