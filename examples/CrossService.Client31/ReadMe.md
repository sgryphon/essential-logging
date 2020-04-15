![Essential Logging](../../docs/images/diagnostics-logo-64.png)

# Cross-service logging (.NET Core 3.1)

.NET now includes automatic correlation of traces across web service tiers.

This example has three components, Client, MidTier, and BackEnd, to show a common correlation TraceId across all three. 

## Running the Example

You need to be running Elasticsearch to log the results to. The [HelloElasticsearch](../HelloElasticsearch) example 
has a docker compose file that can be used to run a local Elasticsearch + Kibana instance.

Start the back end:

```powershell
dotnet run --project ./examples/CrossService.BackEnd31
```

Then the mid tier proxy:

```powershell
dotnet run --project ./examples/CrossService.MidTier31
```

Then run the client:

```powershell
dotnet run --project ./examples/CrossService.Client31
```

In Kibana, add the `trace.id` and `transaction.id` fields to see the correlated traces.

ASP.NET also logs the top level values in the scope as `labels.TraceId` and `labels.SpanId`.
