namespace NotificationService.Infrastructure.Services;

using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Contracts;
using NotificationService.Infrastructure.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

public class RabbitMQConsumer : IMessageConsumer, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection = null!;
    private IChannel _channel = null!;
    private readonly ILogger _logger;
    private readonly RabbitMQOptions _options;

    public RabbitMQConsumer(IOptions<RabbitMQOptions> options)
    {
        this._options = options.Value;

        this._factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password,
            ConsumerDispatchConcurrency = 1,
        };
    }

    public async Task InitializeAsync()
    {
        int maxRetries = 15;
        for (int i = 1; i <= maxRetries; i++)
        {
            try
            {
                this._connection = await this._factory.CreateConnectionAsync();
                this._channel = await this._connection.CreateChannelAsync();
                Console.WriteLine("Connected to RabbitMQ");
                break;
            }
            catch (BrokerUnreachableException ex)
            {
                Console.WriteLine($"Attempt {i} failed: {ex.Message}");
                if (i == maxRetries)
                {
                    throw;
                }

                await Task.Delay(3000);
            }
        }
    }

    public async Task StartConsumingAsync(string queueKey, Func<string, Task> handleMessage, CancellationToken cancellationToken)
    {
        string queueName;
        switch (queueKey)
        {
            case "FriendRequest":
                queueName = this._options.Queues.FriendRequest;
                break;
            case "ProfileCreated":
                queueName = this._options.Queues.ProfileCreated;
                break;
            case "MeetingRequest":
                queueName = this._options.Queues.MeetingRequest;
                break;
            default:
                this._logger.LogError($"Queue key '{queueKey}' is not supported");
                return;
        }

        await this._channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(this._channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await handleMessage(message);
        };

        await this._channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (this._connection is { IsOpen: true })
        {
            await this._connection.CloseAsync();
            this._connection.Dispose();
        }
    }
}
