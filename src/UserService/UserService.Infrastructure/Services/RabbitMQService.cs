namespace UserService.Infrastructure.Services;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using UserService.Application.Contracts;
using UserService.Application.DTOs.Notifications;
using UserService.Infrastructure.Options;

public class RabbitMQService : IMessageService, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMQService> _logger;
    private readonly RabbitMQOptions _options;

    public RabbitMQService(IOptions<RabbitMQOptions> options, ILogger<RabbitMQService> logger)
    {
        this._options = options.Value;
        this._factory = new ConnectionFactory
        {
            HostName = this._options.HostName,
            Port = this._options.Port,
            UserName = this._options.UserName,
            Password = this._options.Password,
        };
        this._connection = this._factory.CreateConnectionAsync().Result;
        this._channel = this._connection.CreateChannelAsync().Result;
        this._logger = logger;
    }

    public async Task PublishFriendRequestAsync(FriendRequestNotification notification)
    {
        var queueName = this._options.Queues.FriendRequest;
        var json = JsonSerializer.Serialize(notification);
        var body = Encoding.UTF8.GetBytes(json);

        await this._channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);

        this._logger.LogInformation($"[RabbitMQ] Message sent to queue '{queueName}': {json}");
    }

    public async Task PublishProfileCreatedAsync(ProfileCreatedNotification notification)
    {
        var queueName = this._options.Queues.ProfileCreated;

        var json = JsonSerializer.Serialize(notification);
        var body = Encoding.UTF8.GetBytes(json);

        await this._channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);

        this._logger.LogInformation($"[RabbitMQ] Message sent to queue '{queueName}': {json}");
    }


    public async ValueTask DisposeAsync()
    {
        if (this._connection is { IsOpen: true })
        {
            await this._connection.CloseAsync();
            this._connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}