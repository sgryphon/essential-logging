# Hello Logging

Let’s introduce a slightly longer example than [getting started](../GettingStarted/ReadMe.md), and we can see what it can do.

Note that this code is intended to demonstrate logging and does not necessarily follow best practices in order to keep things simple, 
although it does try to include some of the logging features.

## Project setup

Create a solution to hold our project:

```powershell
dotnet new solution --name Essential.Logging.Examples
```

Create the new project and add it to the solution:

```powershell
dotnet new console --output HelloLogging
dotnet sln Essential.Logging.Examples.sln add HelloLogging
```

Add a reference to the Microsoft hosting package:

```powershell
dotnet add HelloLogging package Microsoft.Extensions.Hosting
```

Best practice is to enable the new nullable reference types language feature in `HelloLogging.csproj`:

```xml
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>
```

## Writing code

First create some helper methods using [LoggerMessage.Define](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggermessage.define) 
for some [high performance logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage):

Using `LoggerMessage` can help keep your logging focussed, and with a good [theory of event IDs](../../docs/Event-Ids.md) 
and good use of [logging levels](../../docs/Logging-Levels.md).

**Log.cs**
```c#
using System;
using Microsoft.Extensions.Logging;

namespace HelloLogging
{
    internal static class Log
    {
        public static readonly Action<ILogger, int, Exception?> ErrorProcessingCustomer =
            LoggerMessage.Define<int>(LogLevel.Error,
                new EventId(5000, nameof(ErrorProcessingCustomer)),
                "Unexpected error processing customer {CustomerId}.");

        public static readonly Action<ILogger, Guid, Exception?> ProcessOrderItem =
            LoggerMessage.Define<Guid>(LogLevel.Information,
                new EventId(1000, nameof(ProcessOrderItem)),
                "Processing order item {ItemId}.");
    }
}
```

Next add an example worker background service, that will log some values, including an exception:

**Worker.cs**
```c#
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelloLogging
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var ipAddress = IPAddress.Parse("2001:db8:85a3::8a2e:370:7334");
            var customerId = 12345;
            var orderId = "PO-56789";
            var dueDate = new DateTime(2020, 1, 2);
            var total = 100;
            var rate = 0;

            using (_logger.BeginScope("IP address {ip}", ipAddress))
            {
                try
                {
                    using (_logger.BeginScope("Customer {CustomerId}, Order {OrderId}, Due {DueDate:yyyy-MM-dd}",
                        customerId, orderId, dueDate))
                    {
                        for (var i = 0; i < 4; i++)
                        {
                            await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken).ConfigureAwait(false);
                            Log.ProcessOrderItem(_logger, Guid.NewGuid(), null);
                        }

                        var points = total / rate;
                    }
                }
                catch (Exception ex)
                {
                    Log.ErrorProcessingCustomer(_logger, customerId, ex);
                }
            }
        }
    }
}
```

Then update the main program to configure logging and use the host builder to run your worker service:

**Program.cs**
```c#
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloLogging
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });

        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }
    }
}
```

Using `Host.CreateDefaultBuilder(args)` will set up some default configuration, and a default console logger. The `configurationBuilder.SetBasePath()` line 
will look for configuration in the output directory.

For configuration add an `appsettings.json` configuration file, which will set a timestamp and include scopes in the output.

**appsettings.json**
```json
{
  "Logging": {
    "Console": {
      "TimestampFormat": "HH:mm:sszz ",
      "IncludeScopes": true
    },
    "LogLevel" : {
      "Default": "Debug",
      "Microsoft": "Information"
    }
  }
}
```

And also need configure `HelloLogging.csproj` to copy the `appsettings.json` file when building the application:

```xml
  <ItemGroup>
    <None Update="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
```

Now, run and you will see lots of debug level output (press Control + C to exit).

```powershell
dotnet run --project HelloLogging
```

You will see that the logging will include not only your own values, but log messages from the Microsoft host infrastructure, such as the console host set up. 
You can change different settings to see the effect, such as setting the LogLevel for Microsoft to Debug, turning colors off, or changing to a more compact `systemd` format.

## Alternative configuration

You can also have override appsetting files for different environments. The standard environments are Development, Staging, and Production, but you
can actually use any value as a name.

For example, you can create a Staging configuration (you also need to set this to copy to the output directory):

**appsettings.Staging.json**
```json
{
  "Logging": {
    "Console": {
      "TimestampFormat": " HH:mm:sszz ",
      "IncludeScopes": true,
      "Format": "Systemd"
    },
    "LogLevel" : {
      "Default": "Debug",
      "Microsoft": "Information"
    }
  }
}
```

You can then run that alternate configuration by passing in the environment on the command line (or, more commonly, by setting an environment variable):

```powershell
dotnet run --project HelloLogging -- --Environment Staging
```

With the basic console logger the information is limited, but is useful to get started. For additional log destinations you can add LoggerProviders.


