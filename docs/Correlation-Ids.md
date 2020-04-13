


## .NET Core 3 ##

https://devblogs.microsoft.com/aspnet/improvements-in-net-core-3-0-for-troubleshooting-and-monitoring-distributed-apps/


    Activity.DefaultIdFormat = ActivityIdFormat.W3C;

    var activity = new Activity("CallToBackend").Start();

    try
    {
        return await _httpClient.GetStringAsync(
                               "http://localhost:5000/weatherforecastproxy");
    }
    finally
    {
        activity.Stop();
    }

OpenTelemetry provides a single set of APIs, libraries, agents, and collector services to capture distributed traces and metrics from your application. You can analyze them using Prometheus, Jaeger, Zipkin, and other observability tools.


    var activity = new Activity("CallToBackend")
        .AddBaggage("appVersion", "v1.0")
        .Start();


## ASP.NET

### ASP.NET 2.2

 "Scopes": [
      "[ConnectionId, 0HLUVG563JRK8]",
      "[RequestId, 0HLUVG563JRK8:00000001], [RequestPath, /], [CorrelationId, ]",
      "[ActionId, 35cb4b74-1bfc-476b-adbc-418f8aaf9308], [ActionName, /Index]"
    ],

### ASP.NET 3.1

    "Scopes": [
      "[RequestId, 0HLUVD9KBTGNA:0000000F], [RequestPath, /], [SpanId, |307d9bfc-41357bcd53a00574.], [TraceId, 307d9bfc-41357bcd53a00574], [ParentId, ]",
      "[ActionId, 09faf975-5249-444a-9fbb-e313c503cf8d], [ActionName, /Index]"
    ],

## Trace Context ##

https://www.w3.org/TR/trace-context/

