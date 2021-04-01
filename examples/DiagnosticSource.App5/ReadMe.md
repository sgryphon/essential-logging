![Essential Logging](../../docs/images/diagnostics-logo-64.png)

# Diagnostics Source Example

An example using DiagnosticsSource to instrument a library,
which is then consumed by a client application to generate
logging.

The library does not use Microsoft.Extensions.Logging (there
is no where to inject a logger), but does raise events
that can be consumed by a diagnostics listener.

This is useful for library components where it does not
make sense to require everything to have an ILogger, but
where you still want diagnostics.

```pwsh
dotnet run --project examples/DiagnosticSource.App5
```

