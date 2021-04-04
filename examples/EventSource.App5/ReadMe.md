![Essential Logging](../../docs/images/diagnostics-logo-64.png)

# Event Source Example

An example using EventSource to instrument a library,
which is then consumed by a client application to generate
logging.

*Library Code*

The library uses a EventSource to raise events
that can be consumed in process by an EventListener,
or out of process via the .NET Core EventPipe
(or platform-specific native tools).

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

Note that the code to forward the EventSource back to ILogger
is rather inefficient, to get some nice output. It would be 
more common to pass events the other way (and there is
a built in EventSourceLoggerProvider that can do this).

*Running the example*

To run the example app:

```pwsh
dotnet run --project examples/EventSource.App5
```

// TODO: Provide instructions how to listen using dotnet-trace

// TODO: Add event counters

See:
* [Event Source Users Guide](https://github.com/microsoft/dotnet-samples/blob/master/Microsoft.Diagnostics.Tracing/EventSource/docs/EventSource.md)
* [Example Lister](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.eventsource.loggingeventsource?view=dotnet-plat-ext-5.0#examples)
