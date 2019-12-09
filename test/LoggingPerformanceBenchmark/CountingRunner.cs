using System;

namespace LoggingPerformanceBenchmark
{
    public class CountingRunner : RunnerBase
    {
        int critical1 = 0;
        int debug1 = 0;
        int debug2 = 0;
        int warning2 = 0;

        protected override void LogCritical1(int id, string message, params object[] data)
        {
            critical1++;
        }

        protected override void LogDebug1(int id, string message, params object[] data)
        {
            debug1++;
        }

        protected override void LogDebug2(int id, string message, params object[] data)
        {
            debug2++;
        }

        protected override void LogWarning2(int id, string message, params object[] data)
        {
            warning2++;
        }

        protected override void Start()
        {
            critical1 = 0;
            debug1 = 0;
            debug2 = 0;
            warning2 = 0;
        }

        protected override void Finish()
        {
            if (Output)
            {
                Console.WriteLine("Trace1 received {0} verbose and {1} critical.", debug1, critical1);
                Console.WriteLine("Trace2 received {0} verbose and {1} warning.", debug2, warning2);
            }
        }
    }
}
