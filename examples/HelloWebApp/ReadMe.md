![Essential Logging](../../docs/images/diagnostics-logo-64.png)

# HelloWebApp

This example is created from the ASP.NET Web App template, with the Rolling File and Elasticsearch loggers added, 
along with an example singleton service and a few logger calls.

## Running the Example

The example can be run from the console (which will use the Kestrel web server). It will start logging to the console
(as well as a rolling file and to Elasticsearch):

```powershell
dotnet run --project ./examples/HelloWebApp
```

You can then view, or refresh, the home page (which will log the application-specific messages):

```
https://localhost:5001/
```

As well as the console window, log messages will be written to the rolling file in bin/debug/netcoreapp3.1,
to the file HelloWebApp-<date>.log, and also to a local Elasticsearch server.

The [HelloElasticsearch](../HelloElasticsearch) example has a docker compose file that can be used to run a local Elasticsearch + Kibana instance.

