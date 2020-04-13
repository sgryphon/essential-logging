


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



## Trace Context ##

https://www.w3.org/TR/trace-context/

