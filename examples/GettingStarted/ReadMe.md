![Essential Logging](../../docs/images/diagnostics-logo-64.png)

# Getting started

The simplest possible example, using the ubiquitous "Hello World".

Create a new project:

```powershell
dotnet new console --output GettingStarted
```

Add a reference to the Microsoft console logging package:

```powershell
dotnet add GettingStarted package Microsoft.Extensions.Logging.Console 
```

Add the `using` at the top of `Program.cs`, and replace the `Main()` function with the following:

**Program.cs**
```c#
using System;
using Microsoft.Extensions.Logging;

namespace GettingStarted
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ILogger<Program> logger = LoggerFactory
                .Create(logging => logging.AddConsole())
                .CreateLogger<Program>();
            logger.LogInformation("Hello World!");
            Console.ReadLine();
        }
    }
}
```

Run the console application:

```powershell
dotnet run --project GettingStarted
```

A simple "Hello World" isn’t however very useful for showing the different capabilities of logging, so the next page will walk you through a [longer example](../HelloLogging) to introduce the range of features available in [Microsoft.Extensions.Logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging).

----

**Next: [Hello Logging](../HelloLogging)**
