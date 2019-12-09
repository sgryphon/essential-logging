using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggingPerformanceBenchmark
{
    public class CountingRunner : RunnerBase
    {
        private Dictionary<int, int> _eventCount;
        
        protected override void LogCritical9101(int id, string message)
        {
            CountEvent(id);
        }

        private void CountEvent(int id)
        {
            _eventCount.TryGetValue(id, out var count);
            _eventCount[id] = count + 1;
        }

        protected override void LogCritical9102(int id, string message)
        {
            CountEvent(id);
        }

        protected override void LogWarning4201(int id, string message, int counter2)
        {
            CountEvent(id);
        }

        protected override void LogWarning4202(int id, string message, int counter2)
        {
            CountEvent(id);
        }

        protected override void LogWarning4203(int id, string message, int counter2)
        {
            CountEvent(id);
        }

        protected override void LogDebug31001(int id, string message, int data1, string data2)
        {
            CountEvent(id);
        }

        protected override void LogDebug31002(int id, string message, int data1)
        {
            CountEvent(id);
        }

        protected override void LogDebug32003(int id, string message, int data1, string data2)
        {
            CountEvent(id);
        }

        protected override void LogDebug32004(int id, string message, int data1)
        {
            CountEvent(id);
        }

        protected override void Start()
        {
            _eventCount = new Dictionary<int, int>();
        }

        protected override void Finish()
        {
            if (Output)
            {
                foreach (var kvp in _eventCount.OrderBy(x => x.Key))
                {
                    Console.WriteLine("Event {0} occurred {1:n0} times.", kvp.Key, kvp.Value);
                }
            }
        }
    }
}
