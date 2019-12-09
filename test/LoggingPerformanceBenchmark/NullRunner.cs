namespace LoggingPerformanceBenchmark
{
    public class NullRunner : RunnerBase
    {
        public override string Name
        {
            get
            {
                return base.Name + " (base time)";
            }
        }

        protected override void LogCritical9101(int id, string message)
        {
        }

        protected override void LogCritical9102(int id, string message)
        {
        }

        protected override void LogWarning4201(int id, string message, int counter2)
        {
        }

        protected override void LogWarning4202(int id, string message, int counter2)
        {
        }

        protected override void LogWarning4203(int id, string message, int counter2)
        {
        }

        protected override void LogDebug31001(int id, string message, int data1, string data2)
        {
        }

        protected override void LogDebug31002(int id, string message, int data1)
        {
        }

        protected override void LogDebug32003(int id, string message, int data1, string data2)
        {
        }

        protected override void LogDebug32004(int id, string message, int data1)
        {
        }
    }
}
