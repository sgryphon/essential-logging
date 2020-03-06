# Hello Speedscope

The default `Host.CreateDefaultBuilder()` also includes a log provider for EventSource, which can be recorded via the `dotnet trace` tool to a Speedscope file.

```powershell
dotnet tool install --global dotnet-trace
```

Open a second console, e.g. PowerShell, and prepare to capture the logging (below).

Run the target application:

```powershell
dotnet run --project HelloLogging
```

Then start logging in the second console, using the process ID of the first:

```powershell
dotnet trace ps
dotnet trace collect -p 500 --format Speedscope --providers Microsoft-Extensions-Logging:4:5:FilterSpecs=\"\"
```

Enable nullable references in the project, copy in the hello world code, and add some logging.

**HelloLogging.cs**
```c#
using System;
    }
}
```

Using `Host.CreateDefaultBuilder()` will set up some default configuration, and a default console logger. The `configurationBuilder.SetBasePath()` line will look for configuration in the output directory.

With the basic console logger the information is limited, but the default settings already include the basics of what you need.

