using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LoggingPerformanceBenchmark
{
    public abstract class RunnerBase
    {
        int[] data1 = new int[] { 1, 2, 3, 5, 8, 13, 21 };
        string[] data2 = new string[] { "alpha", "beta", "gamma" };
        TimeSpan elapsed;

        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public TimeSpan BaseTime { get; set; }

        public TimeSpan Elapsed { get { return elapsed; } }

        public int Iterations { get; set; }

        public bool Output { get; set; }
        
        public void Run()
        {
            var spacer30 = new string(' ', 30);
            if (Output)
            {
                //Console.WriteLine("");
                //Console.WriteLine("{2:s} start (x{1}) {0}.", Name, Iterations, DateTimeOffset.Now);
                Console.Write("{0,-30} (x{1,8}) : ", Name, Iterations);
            }
            Start();
            Stopwatch sw = Stopwatch.StartNew();

            var countCritical = 0;
            for (int index = 0; index < Iterations; index++)
            {
                var counter1 = index & 0x0FFF;
                var counter2 = (index >> 1) & 0x0FFF;
                var index1 = index % 7;
                var index2 = index % 3;

                if ((index & 0xffff) == 0)
                {
                    // Log critical every 64,000 messages
                    LogCritical1(9000 + countCritical++, "Message (critical error).");
                }
                else if ((index & 0x0fff) == 0)
                {
                    // Log warning every 4,000 messages
                    LogWarning2(4000 + index1 + index2, "Message (warning {Counter}).", counter2);
                }
                else
                {
                    // Log verbose
                    if ((index & 0xFF) == 0)
                    {
                        if ((index & 0x1) == 0)
                        {
                            LogDebug1(counter1, "Message 1 is {Data1} + {Data2}.", data1[index1], data2[index2]);
                        }
                        else
                        {
                            LogDebug1(counter2, "Message 2 is {Data1}.", data1[index1]);
                        }
                    }
                    else
                    {
                        if ((index & 0x1) == 0)
                        {
                            LogDebug2(counter1, "Message 1 is {Data1} + {Data2}.", data1[index1], data2[index2]);
                        }
                        else
                        {
                            LogDebug2(counter2, "Message 2 is {Data1}.", data1[index1]);
                        }
                    }
                }
            }

            sw.Stop();
            elapsed = sw.Elapsed;
            if (Output)
            {
                //Console.WriteLine("{1:s} stop {0}.", Name, DateTimeOffset.Now);
                var difference = Elapsed - BaseTime;
                Console.WriteLine("{0,12:f4}", difference.TotalMilliseconds);
            }
            Finish();
        }

        protected abstract void LogCritical1(int id, string message, params object[] data);
        protected abstract void LogDebug1(int id, string message, params object[] data);

        protected abstract void LogDebug2(int id, string message, params object[] data);
        protected abstract void LogWarning2(int id, string message, params object[] data);

        protected virtual void Start() { }
        protected virtual void Finish() { }

    }
}
