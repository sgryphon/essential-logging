using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LoggingPerformanceBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            ILoggerFactory clearProvidersFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .ClearProviders()
                    .AddFilter("Events.None1", LogLevel.None)
                    .AddFilter("Events.None2", LogLevel.None)
                    .AddFilter("Events.Warning1", LogLevel.Warning)
                    .AddFilter("Events.Warning2", LogLevel.Warning)
                    .AddFilter("Events.Trace1", LogLevel.Trace);
            });
            ILoggerFactory eventSourceFactory = LoggerFactory.Create(builder =>
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
            NullLoggerFactory nullLoggerFactory = new NullLoggerFactory();

            int warmupCount = 1000;
            int iterations = 1000000;
            //var iterations = 1000;
            NullRunner nullRunner = new NullRunner();
            RunnerBase[] runners = new RunnerBase[] {
                new CountingRunner(),
                new LoggerRunner("Eventing - One Full", eventSourceFactory, "Events.Trace1", "Events.Warning2"),
                new LoggerRunner("Eventing - Two Filt.", eventSourceFactory, "Events.Warning1", "Events.Warning2"),
                new LoggerRunner("Eventing - One Filt.", eventSourceFactory, "Events.Warning1", "Events.None2"),
                new LoggerRunner("Eventing - None", eventSourceFactory, "Events.None1", "Events.None2"),
                
                new LoggerMessageRunner("Eventing - One Full", eventSourceFactory, "Events.Trace1", "Events.Warning2"),
                new LoggerMessageRunner("Eventing - Two Filt.", eventSourceFactory, "Events.Warning1", "Events.Warning2"),
                new LoggerMessageRunner("Eventing - One Filt.", eventSourceFactory, "Events.Warning1", "Events.None2"),
                new LoggerMessageRunner("Eventing - None", eventSourceFactory, "Events.None1", "Events.None2"),
                
                new GuardedLoggerRunner("Eventing - One Full", eventSourceFactory, "Events.Trace1", "Events.Warning2"),
                new GuardedLoggerRunner("Eventing - Two Filt.", eventSourceFactory, "Events.Warning1", "Events.Warning2"),
                new GuardedLoggerRunner("Eventing - One Filt.", eventSourceFactory, "Events.Warning1", "Events.None2"),
                new GuardedLoggerRunner("Eventing - None", eventSourceFactory, "Events.None1", "Events.None2"),
                
                new LoggerMessageRunner("Eventing2 - One Full", eventSourceFactory, "Events.Trace1", "Events.Warning2"),
                new LoggerMessageRunner("Eventing2 - Two Filt.", eventSourceFactory, "Events.Warning1", "Events.Warning2"),
                new LoggerMessageRunner("Eventing2 - One Filt.", eventSourceFactory, "Events.Warning1", "Events.None2"),
                new LoggerMessageRunner("Eventing2 - None", eventSourceFactory, "Events.None1", "Events.None2"),
                
                new GuardedLoggerRunner("Eventing2 - One Full", eventSourceFactory, "Events.Trace1", "Events.Warning2"),
                new GuardedLoggerRunner("Eventing2 - Two Filt.", eventSourceFactory, "Events.Warning1", "Events.Warning2"),
                new GuardedLoggerRunner("Eventing2 - One Filt.", eventSourceFactory, "Events.Warning1", "Events.None2"),
                new GuardedLoggerRunner("Eventing2 - None", eventSourceFactory, "Events.None1", "Events.None2"),

                new LoggerRunner("Eventing2 - One Full", eventSourceFactory, "Events.Trace1", "Events.Warning2"),
                new LoggerRunner("Eventing2 - Two Filt.", eventSourceFactory, "Events.Warning1", "Events.Warning2"),
                new LoggerRunner("Eventing2 - One Filt.", eventSourceFactory, "Events.Warning1", "Events.None2"),
                new LoggerRunner("Eventing2 - None", eventSourceFactory, "Events.None1", "Events.None2"),

                new LoggerRunner("ClearProviders - One Full", clearProvidersFactory, "Events.Trace1", "Events.Warning2"),
                new LoggerRunner("ClearProviders - Two Filt.", clearProvidersFactory, "Events.Warning1", "Events.Warning2"),
                new LoggerRunner("ClearProviders - One Filt.", clearProvidersFactory, "Events.Warning1", "Events.None2"),
                new LoggerRunner("ClearProviders - None", clearProvidersFactory, "Events.None1", "Events.None2"),

                new LoggerRunner("Null - One Full", nullLoggerFactory, "Events.Trace1", "Events.Warning2"),
                new LoggerRunner("Null - Two Filt.", nullLoggerFactory, "Events.Warning1", "Events.Warning2"),
                new LoggerRunner("Null - One Filt.", nullLoggerFactory, "Events.Warning1", "Events.None2"),
                new LoggerRunner("Null - None", nullLoggerFactory, "Events.None1", "Events.None2"),
            };

            Console.WriteLine("Logging performance tester.");
            Console.WriteLine("- Sends {0} warmup trace messages.", warmupCount);
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
            nullRunner.Iterations = warmupCount;
            nullRunner.Run();
            foreach (RunnerBase runner in runners)
            {
                runner.Output = false;
                runner.Iterations = warmupCount;
                runner.Run();
            }

            Console.WriteLine("Running performance tests:");
            Console.WriteLine("");
            nullRunner.Output = true;
            nullRunner.Iterations = iterations;
            nullRunner.Run();

            Console.WriteLine("");
            foreach (RunnerBase runner in runners)
            {
                runner.BaseTime = TimeSpan.Zero; // nullRunner.Elapsed;
                runner.Output = true;
                runner.Iterations = iterations;
                runner.Run();
            }

            Console.WriteLine("");
            nullRunner.Output = true;
            nullRunner.Iterations = iterations;
            nullRunner.Run();

            Console.WriteLine();
            Console.WriteLine("Done");
            //Console.ReadLine();
        }
    }
}
