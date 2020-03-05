# Logging Primer

A simple ["Hello" example](../README.md) isn’t however very useful for showing the different capabilities of logging, so we will use a slightly more complicated example. First of all, here is the program we will use both without and then with simple logging.

* Hello World with no logging (see below)
* [Hello Logging](Hello-Logging.md)

Once the program has had logging statements added, any of the logging providers can be configured to send the log output to a wide variety of destinations.

# Hello World (no logging)

This version of “Hello World” involves a bunch of Worker classes that Poke() each other to say “Hello World”. Sometimes they get sick of being poked.

Create a solution to hold our project:

```powershell
dotnet new solution --name Essential.Logging.Examples
```

Create the new project and add to the solution:

```powershell
dotnet new console --output HelloWorld
dotnet sln add HelloWorld
```

Add a reference to the Microsoft hosting package:

```powershell
dotnet add HelloWorld package Microsoft.Extensions.Hosting
```

Best practice is to enable the new nullable reference types language feature in HelloWorld.csproj:

```xml
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>
```

Replace the contents of the application with the following, using the [.NET Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host):

**Program.cs**
```c#
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloWorld
{
    public class Program
    {
        public static Random Random = new Random();
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .UseConsoleLifetime()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<WorkerRegistry>();
                    int numberOfWorkers = Random.Next(2, 4);
                    for (int i = 1; i <= numberOfWorkers; i++)
                    {
                        services.AddSingleton<IHostedService, Worker>();
                    }
                });

        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }
    }

    public class WorkerRegistry
    {
        public IList<Worker> Workers { get; } = new List<Worker>();
    }

    public class Worker : BackgroundService
    {
        private readonly WorkerRegistry _registry;
        private int _pokedCount;

        public Worker(WorkerRegistry registry)
        {
            _registry = registry;
            _registry.Workers.Add(this);
        }

        public async Task PokeAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(Program.Random.Next(500)), stoppingToken);
            if (++_pokedCount < 4)
                Console.WriteLine("Hello World {0}", _pokedCount);
            else if (_pokedCount < 6)
                Console.WriteLine("Hi");
            else
                throw new Exception("Too many pokes");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(Program.Random.Next(500)), stoppingToken);
                int index = Program.Random.Next(_registry.Workers.Count);
                await _registry.Workers[index].PokeAsync(stoppingToken);
            }
        }
    }
}
```

Running this program (Ctrl+C to exit) may produce the following:

```powershell
PS> dotnet run --project HelloWorld
Hello World 1
Hello World 2
Hello World 1
Hello World 3
Hello World 2
Hi
Hi
Hello World 3
Hi
Hi
^C
```

Lots of “Hello World”, but a bit difficult to tell which bit of code did what.

**Next: [Hello Logging](Hello-Logging.md)**
