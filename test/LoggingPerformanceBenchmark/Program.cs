using System;
using Microsoft.Extensions.Logging;

namespace LoggingPerformanceBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var clearProvidersFactory = LoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
            });
            var eventSourceFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .ClearProviders()
                    .AddFilter("Events.None1", LogLevel.None)
                    .AddFilter("Events.None2", LogLevel.None)
                    .AddFilter("Events.Warning1", LogLevel.Warning)
                    .AddFilter("Events.Warning2", LogLevel.Warning)
                    .AddFilter("Events.Trace1", LogLevel.Trace)
                    .AddEventSourceLogger();
            });
            
            var iterations = 1000000;
            //var iterations = 1000;
            var nullRunner = new NullRunner();
            var runners = new RunnerBase[] {
                new CountingRunner(),
                new LoggerRunner("ClearProviders", clearProvidersFactory, "NoSource1", "NoSource2"),
                new LoggerRunner("Eventing - None", eventSourceFactory, "Events.None1", "Events.None2"),
                new LoggerRunner("Eventing - One Filt.", eventSourceFactory, "Events.Warning1", "Events.None2"),
                new LoggerRunner("Eventing - Two Filt.", eventSourceFactory, "Events.Warning1", "Events.Warning2"),
                new LoggerRunner("Eventing - One Full", eventSourceFactory, "Events.Trace1", "Events.Warning2"),
                new GuardedLoggerRunner("Eventing - None", eventSourceFactory, "Events.None1", "Events.None2"),
                new GuardedLoggerRunner("Eventing - One Filt.", eventSourceFactory, "Events.Warning1", "Events.None2"),
                new GuardedLoggerRunner("Eventing - Two Filt.", eventSourceFactory, "Events.Warning1", "Events.Warning2"),
                new GuardedLoggerRunner("Eventing - One Full", eventSourceFactory, "Events.Trace1", "Events.Warning2"),
                new MisusedLoggerMessageRunner("Eventing - None", eventSourceFactory, "Events.None1", "Events.None2"),
                new MisusedLoggerMessageRunner("Eventing - One Filt.", eventSourceFactory, "Events.Warning1", "Events.None2"),
                new MisusedLoggerMessageRunner("Eventing - Two Filt.", eventSourceFactory, "Events.Warning1", "Events.Warning2"),
                new MisusedLoggerMessageRunner("Eventing - One Full", eventSourceFactory, "Events.Trace1", "Events.Warning2"),
            };

            Console.WriteLine("Logging performance tester.");
            Console.WriteLine("- Sends 10 warmup trace messages.");
            Console.WriteLine("- Then times the send of {0} trace messages.", iterations);
            Console.WriteLine("");
            Console.WriteLine("Typical configurations tested for each logging system:");
            Console.WriteLine("- Logging turned off (ignore all messages)", iterations);
            Console.WriteLine("- Warnings logged for source 1", iterations);
            Console.WriteLine("- Warnings logged for both sources", iterations);
            Console.WriteLine("- All source 1, warnings for source 2", iterations);
            Console.WriteLine("");
            Console.WriteLine("Times (in milliseconds) show difference compared to a NullRunner that does nothing,");
            Console.WriteLine("i.e. to eliminate the overhead time taken for the actual looping.");

            Console.WriteLine("");
            Console.WriteLine("Warming up...");
            nullRunner.Output = false;
            nullRunner.Iterations = 10;
            nullRunner.Run();
            foreach (var runner in runners)
            {
                runner.Output = false;
                runner.Iterations = 10;
                runner.Run();
            }

            Console.WriteLine("Running performance tests:");
            Console.WriteLine("");
            nullRunner.Output = true;
            nullRunner.Iterations = iterations;
            nullRunner.Run();

            Console.WriteLine("");
            Console.WriteLine("Delta from null runner:");

            foreach (var runner in runners)
            {
                runner.BaseTime = nullRunner.Elapsed;
                runner.Output = true;
                runner.Iterations = iterations;
                runner.Run();
            }

            Console.WriteLine();
            Console.WriteLine("Done");
            //Console.ReadLine();
        }
    }
}
