using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrossService.Client31
{
    class Program
    {
        private static HttpClient _httpClient;
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("Cross-service correlation example");
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            _httpClient = new HttpClient();
            var forecast = await GetWeatherForecast();
            Console.WriteLine(forecast);
        }
        
        private static async Task<string> GetWeatherForecast()
        {
            var activity = new Activity("CallToBackend").Start();
            Console.WriteLine("Activity.Id={0}", activity.Id);
            Console.WriteLine("Activity.ParentId={0}", activity.ParentId);
            Console.WriteLine("Activity.RootId={0}", activity.RootId);
            Console.WriteLine("Activity.SpanId={0}", activity.SpanId);
            Console.WriteLine("Activity.TraceId={0}", activity.TraceId);

            try
            {
                return await _httpClient.GetStringAsync(
                    "http://localhost:5000/weatherforecastproxy");
            }
            finally
            {
                activity.Stop();
            }
        }
    }
}
