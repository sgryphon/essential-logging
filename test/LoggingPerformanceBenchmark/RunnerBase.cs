using System;
using System.Diagnostics;

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

            for (int index = 0; index < Iterations; index++)
            {
                SelectLogMessage(index);
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

        protected virtual void SelectLogMessage(int index)
        {
            var counter2 = (index >> 1) & 0x0FFF;
            var index1 = index % 7;
            var index2 = index % 3;

            // Log critical every 64,000 messages
            if ((index & 0x1ffff) == 0)
            { 
                LogCritical9101(9101, "Message (critical error).");
            }
            else if ((index & 0xffff) == 0)
            {
                LogCritical9102(9102, "Message (critical error).");
            }
            
            // Log warning every 4,000 messages
            else if ((index & 0x0fff) == 0 && index2 == 0)
            {
                LogWarning4201(4201, "Message (warning {Counter}).", counter2);
            }
            else if ((index & 0x0fff) == 0 && index2 == 1)
            {
                LogWarning4202(4202, "Message (warning {Counter}).", counter2);
            }
            else if ((index & 0x0fff) == 0)
            {
                LogWarning4203(4203, "Message (warning {Counter}).", counter2);
            }
            
            // Log verbose
            else if ((index & 0x1FF) == 0)
            {
                LogDebug31001(31001, "Message 1 is {Data1} + {Data2}.", data1[index1], data2[index2]);
            }
            else if ((index & 0xFF) == 0)
            {
                LogDebug31002(31002, "Message 2 is {Data1}.", data1[index1]);
            }
            else if ((index & 0x1) == 0)
            {
                LogDebug32003(32003, "Message 1 is {Data1} + {Data2}.", data1[index1], data2[index2]);
            }
            else
            {
                LogDebug32004(32004, "Message 2 is {Data1}.", data1[index1]);
            }
        }

        protected abstract void LogCritical9101(int id, string message);
        protected abstract void LogCritical9102(int id, string message);
        
        protected abstract void LogWarning4201(int id, string message, int counter2);
        protected abstract void LogWarning4202(int id, string message, int counter2);
        protected abstract void LogWarning4203(int id, string message, int counter2);
        
        protected abstract void LogDebug31001(int id, string message, int data1, string data2);
        protected abstract void LogDebug31002(int id, string message, int data1);
        protected abstract void LogDebug32003(int id, string message, int data1, string data2);
        protected abstract void LogDebug32004(int id, string message, int data1);

        protected virtual void Start() { }
        protected virtual void Finish() { }

    }
}
