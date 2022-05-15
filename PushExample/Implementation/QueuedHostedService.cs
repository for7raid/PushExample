using PushExample.Abstraction;
using PushExample.Models;

namespace PushExample.Implementation
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger<QueuedHostedService> _logger;

        public QueuedHostedService(IBackgroundTaskQueue taskQueue,
            IServiceProvider services,
            ILogger<QueuedHostedService> logger)
        {
            TaskQueue = taskQueue;
            Services = services;
            _logger = logger;
        }

        public IBackgroundTaskQueue TaskQueue { get; }
        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                $"Queued Hosted Service is running.{Environment.NewLine}");

            //Запустим 4 процесса для параллельной отправки сообщений из очереди
            await Task.WhenAll(BackgroundProcessing(stoppingToken), BackgroundProcessing(stoppingToken), BackgroundProcessing(stoppingToken), BackgroundProcessing(stoppingToken));
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var messageId =
                    await TaskQueue.DequeueAsync(stoppingToken);

                try
                {
                    using (var scope = Services.CreateScope())
                    {
                        var serivce =
                            scope.ServiceProvider
                                .GetRequiredService<IMessageService>();
                        await serivce.SendAsync(messageId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error occurred executing {WorkItem}.", nameof(messageId));
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
