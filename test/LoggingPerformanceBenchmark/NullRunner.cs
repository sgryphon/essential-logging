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
        
        protected override void LogCritical1(int id, string message, params object[] data)
        {
        }

        protected override void LogDebug1(int id, string message, params object[] data)
        {
        }

        protected override void LogDebug2(int id, string message, params object[] data)
        {
        }

        protected override void LogWarning2(int id, string message, params object[] data)
        {
        }
    }
}
