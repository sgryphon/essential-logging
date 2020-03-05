using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloWorld
{
    internal class Program
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
                        services.AddHostedService<Worker>();
                    }
                });

        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }
    }

    public class WorkerRegistry
    {
        private readonly List<Worker> _workers = new List<Worker>();

        public IReadOnlyList<Worker> Workers { get; }

        public WorkerRegistry()
        {
            Workers = _workers.AsReadOnly();
        }

        public int AddWorker(Worker worker) 
        {
            lock (((ICollection)_workers).SyncRoot)
            {
                _workers.Add(worker);
                return _workers.Count - 1;
            }
        }
    }

    public class Worker : BackgroundService
    {
        private readonly WorkerRegistry _registry;
        private int _pokedCount;

        public Worker(WorkerRegistry registry)
        {
            _registry = registry;
            int index = _registry.AddWorker(this);
            Id = index;
        }

        public int Id { get; }

        public async Task PokeAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(Program.Random.Next(500)), stoppingToken)
                .ConfigureAwait(false);
            _pokedCount++;
            if (_pokedCount < 4)
                Console.WriteLine("Hello World {0}", _pokedCount);
            else if (_pokedCount < 6)
            {
                Console.WriteLine("Hi");
            }
            else
            {
                throw new Exception("Too many pokes");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(Program.Random.Next(500)), stoppingToken)
                    .ConfigureAwait(false);
                int index = Program.Random.Next(_registry.Workers.Count);
                await _registry.Workers[index].PokeAsync(stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
