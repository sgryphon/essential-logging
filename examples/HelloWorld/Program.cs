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
