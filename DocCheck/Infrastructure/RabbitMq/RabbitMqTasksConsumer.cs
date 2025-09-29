using DocCheck.Application;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DocCheck.Infrastructure.RabbitMq
{
    public class RabbitMqTasksConsumer(
        IConfiguration configuration,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<RabbitMqTasksConsumer> logger) : BackgroundService
    {
        private readonly RabbitMqConfig _config = configuration.GetSection(RabbitMqConfig.Section).Get<RabbitMqConfig>()
                ?? throw new InvalidOperationException("RabbitMq Config not found");
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<RabbitMqTasksConsumer> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_config.IsUse)
            {
                _logger.LogWarning("RabbitMq Service is not use");
                return;
            }

            if (_config.IsWrong)
                throw new InvalidOperationException("RabbitMq Config is wrong");

            var factory = new ConnectionFactory()
            {
                HostName = _config.HostName!,
                UserName = _config.UserName!,
                Password = _config.Password!
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogDebug("RabbitMq Tasks Consumer is running");

                    using var connection = await factory.CreateConnectionAsync(cancellationToken: stoppingToken);

                    using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);


                    string tasksExchange = "tasks";
                    await channel.ExchangeDeclareAsync(
                        exchange: tasksExchange,
                        type: ExchangeType.Fanout,
                        durable: true,
                        autoDelete: false, 
                        cancellationToken: stoppingToken);


                    string docCheckQueue = "doc-check-task";
                    await channel.QueueDeclareAsync(
                        queue: docCheckQueue, 
                        durable: true, 
                        exclusive: false, 
                        autoDelete: false, 
                        cancellationToken: stoppingToken);

                    await channel.QueueBindAsync(
                        queue: docCheckQueue, 
                        exchange: tasksExchange, 
                        routingKey: "", 
                        cancellationToken: stoppingToken);

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        byte[] body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        _logger.LogDebug("RabbitMq Tasks Consumer Received {DeliveryTag}", ea.DeliveryTag);

                        try
                        {
                            await HandleMessage(message);

                            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                            _logger.LogDebug("RabbitMq Tasks Consumer BasicAckAsync {DeliveryTag}", ea.DeliveryTag);
                        }
                        catch (Exception)
                        {
                            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                            _logger.LogWarning("RabbitMq Tasks Consumer BasicNackAsync {DeliveryTag}", ea.DeliveryTag);
                        }
                    };

                    await channel.BasicConsumeAsync(
                        queue: docCheckQueue,
                        autoAck: false,
                        consumer: consumer,
                        cancellationToken: stoppingToken);

                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "RabbitMq Tasks Consumer Error");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        private async Task HandleMessage(string message)
        {
            using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();

            var saleDocService = serviceScope.ServiceProvider.GetService<ISaleDocService>()
                ?? throw new InvalidOperationException("SaleDocService is null");

            await saleDocService.HandleManagerTaskAsync(message);
        }
    }
}
