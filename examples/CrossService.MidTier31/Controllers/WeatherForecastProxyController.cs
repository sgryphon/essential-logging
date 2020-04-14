using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CrossService.MidTier31.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastProxyController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public WeatherForecastProxyController(ILogger<WeatherForecastProxyController> logger,
            HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            _logger.LogInformation(2200,
                "MidTier Controller Id {ActId}, TraceId {ActTraceId}, SpanId {ActSpanId}, ParentId {ActParentId}, RootId {ActRootId}",
                Activity.Current.Id, Activity.Current.TraceId, Activity.Current.SpanId, Activity.Current.ParentId,
                Activity.Current.RootId);

            var jsonStream = await 
                _httpClient.GetStreamAsync("http://localhost:5000/weatherforecast");

            var weatherForecast = await 
                JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>>(jsonStream);

            return weatherForecast;
        }
    }
}
