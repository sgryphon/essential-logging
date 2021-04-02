![Essential Logging](../../docs/images/diagnostics-logo-64.png)

# Diagnostics Source Example

An example using DiagnosticsSource to instrument a library,
which is then consumed by a client application to generate
logging.

*Library Code*

The library uses a DiagnosticSource to raise events
that can be consumed by a diagnostics listener, including
Activity start and stop events.

There is no capability to inject an ILogger, or any
assumption about what logging will be used.

This is useful for library components where it does not
make sense to require everything to have an ILogger, but
where you still want diagnostics.

*Application Code*

Code that uses the library, whether directly in an application
or a component that supports dependency injection, can 
configure to listen to the events and forward them to
the logging.

This can be used in either an application itself, which has a host,
logger, and dependency injection, or can be included in a 
component that is intended to be used with dependency injection
(that will have an ILogger).

*Running the example*

To run the example app:

```pwsh
dotnet run --project examples/DiagnosticSource.App5
```

See:
* [Diagnostic Source Users Guide](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Diagnostics.DiagnosticSource/src/DiagnosticSourceUsersGuide.md)
* [Activity Users Guide](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Diagnostics.DiagnosticSource/src/ActivityUserGuide.md)
