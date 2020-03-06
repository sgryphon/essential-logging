using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelloLogging
{
    public class Program
    {
        public static Random Random = new Random();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder()
                .UseConsoleLifetime()
                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                })
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
        public int _id;
        public IList<Worker> Workers { get; } = new List<Worker>();
        public int NextId() => Interlocked.Increment(ref _id);
    }

    public class Worker : BackgroundService
    {
        private readonly int _id;
        private readonly ILogger _logger;
        private int _pokedCount;
        private readonly WorkerRegistry _registry;

        public Worker(ILogger<Worker> logger, WorkerRegistry registry)
        {
            _logger = logger;
            _registry = registry;
            _id = _registry.NextId();
            _registry.Workers.Add(this);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (_logger.BeginScope($"worker {_id}"))
            {
                _logger.LogInformation(1500, "Worker {0} started", _id);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(Program.Random.Next(500)), stoppingToken);
                        int index = Program.Random.Next(_registry.Workers.Count);
                        _logger.LogDebug("Worker {WorkerId} will poke {TargetIndex}", _id, index);
                        await _registry.Workers[index].PokeAsync(stoppingToken);
                    }
                }
                finally
                {
                    _logger.LogInformation(8500, "Worker {0} ending", _id);
                }
            }
        }

        public async Task PokeAsync(CancellationToken stoppingToken)
        {
            using (_logger.BeginScope($"poke {_id}"))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(Program.Random.Next(500)), stoppingToken);
                if (++_pokedCount < 4)
                    Console.WriteLine("Hello World {0}", _pokedCount);
                else if (_pokedCount < 6)
                    _logger.LogWarning(4500, "Worker {WorkerId} getting annoyed", _id);
                else
                    _logger.LogError(5500, "Worker {WorkerId} too many pokes", _id);
            }
        }
    }
}
