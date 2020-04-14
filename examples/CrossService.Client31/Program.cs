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
            _httpClient = new HttpClient();
            var forecast = await GetWeatherForecast();
            Console.WriteLine(forecast);
        }
        
        private static async Task<string> GetWeatherForecast()
        {
            var activity = new Activity("CallToBackend").Start();
            Console.WriteLine("Activity.Id={0}", activity.Id);

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
