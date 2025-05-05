namespace EventsService.Infrastructure.Services;

using System.Text;
using System.Text.Json;
using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Notifications;
using EventsService.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

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

    public Task PublishMeetingRequest(MeetingRequestNotification notification)
    {
        var queueName = this._options.Queues.MeetingRequest;
        var json = JsonSerializer.Serialize(notification);
        var body = Encoding.UTF8.GetBytes(json);

        this._channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        this._channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);

        this._logger.LogInformation($"[RabbitMQ] Message sent to queue '{queueName}': {json}");
        return Task.CompletedTask;
    }

    public Task PublishGoalRequest(GoalRequestNotification notification)
    {
        var queueName = this._options.Queues.GoalRequest;
        var json = JsonSerializer.Serialize(notification);
        var body = Encoding.UTF8.GetBytes(json);

        this._channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        this._channel.BasicPublish(
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