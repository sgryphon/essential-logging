![Essential Logging](../../docs/images/diagnostics-logo-64.png)

# Activity Source Example

An example using ActivitySource to instrument a library,
which is then consumed by a client application to generate
logging.

*Library Code*

The library uses a ActivitySource to create activities
that can be consumed by am ActivityListener, including
start and stop, as well as custom tags and events.

There is no capability to inject an ILogger, or any
assumption about what logging will be used.

This is useful for library components where it does not
make sense to require everything to have an ILogger, but
where you still want diagnostics.

*Application Code*

Code that uses the library, whether directly in an application
or a component that supports dependency injection, can 
configure to listen to the activities and forward them to
the logging.

This can be used in either an application itself, which has a host,
logger, and dependency injection, or can be included in a 
component that is intended to be used with dependency injection
(that will have an ILogger).

Note that activities are only logged on start and stop, so the list
of events is only available when stop occurs. The activity stop
may also have more details, e.g. more tags.

*Running the example*

To run the example app:

```pwsh
dotnet run --project examples/ActivitySource.App5
```

See:
* [Distributed Tracing Collection, using custom logic](https://docs.microsoft.com/en-gb/dotnet/core/diagnostics/distributed-tracing-collection-walkthroughs#collect-traces-using-custom-logic)
* [A Lap Around ActivitySource and ActivityListener](https://jimmybogard.com/activitysource-and-listener-in-net-5/)
