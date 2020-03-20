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
