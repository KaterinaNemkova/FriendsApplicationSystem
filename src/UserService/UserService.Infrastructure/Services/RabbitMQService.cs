

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
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQService> _logger;
    private readonly RabbitMQOptions _options;

    public RabbitMQService(IOptions<RabbitMQOptions> options, ILogger<RabbitMQService> logger)
    {
        this._options = options.Value;
        var factory = new ConnectionFactory()
        {
            HostName = this._options.HostName,
            Port = this._options.Port,
            UserName = this._options.UserName,
            Password = this._options.Password,
        };
        this._logger = logger;
        this._logger.LogInformation("RabbitMQ Options: Host={Host}, Port={Port}, User={User}", _options.HostName, _options.Port, _options.UserName);

        this._connection = factory.CreateConnection();
        this._channel = this._connection.CreateModel();
    }

    public Task PublishFriendRequest(FriendRequestNotification notification)
    {
        var queueName = this._options.Queues.FriendRequest;
        var json = JsonSerializer.Serialize(notification);
        var body = Encoding.UTF8.GetBytes(json);

        this._channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);

        this._logger.LogInformation($"[RabbitMQ] Message sent to queue '{queueName}': {json}");
        return Task.CompletedTask;
    }

    public Task PublishProfileCreated(ProfileCreatedNotification notification)
    {
        if (notification == null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        var queueName = this._options.Queues.ProfileCreated;
        if (string.IsNullOrEmpty(queueName))
        {
            throw new InvalidOperationException("Queue name is not configured");
        }

        var json = JsonSerializer.Serialize(notification);
        var body = Encoding.UTF8.GetBytes(json);

        this._channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);

        this._logger.LogInformation($"[RabbitMQ] Message sent to queue '{queueName}': {json}");
        return Task.CompletedTask;
    }


    public ValueTask DisposeAsync()
    {
        if (this._connection is { IsOpen: true })
        {
            this._connection.Close();
            this._connection.Dispose();
            GC.SuppressFinalize(this);
        }

        return default;
    }
}