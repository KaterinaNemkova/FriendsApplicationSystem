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
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMQService> _logger;
    private readonly RabbitMQOptions _options;

    public RabbitMQService(IOptions<RabbitMQOptions> options, ILogger<RabbitMQService> logger)
    {
        _options = options.Value;
        _logger = logger;
        try
        {
            _factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
            };

            _logger.LogInformation("RabbitMQ Options: Host={Host}, Port={Port}, User={User}", _options.HostName, _options.Port, _options.UserName);

            _connection = _factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while establishing the RabbitMQ connection.");
            throw;
        }
    }

    public async Task PublishMeetingRequest(RequestNotification notification)
    {
        var queueName = this._options.Queues.MeetingRequest;
        await PublishRequest(queueName, notification);
    }

    public async Task PublishGoalRequest(RequestNotification notification)
    {
        var queueName = _options.Queues.GoalRequest;
        await PublishRequest(queueName, notification);
    }

    public async Task PublishDateRequest(RequestNotification notification)
    {
        var queueName = _options.Queues.DateRequest;
        await PublishRequest(queueName, notification);
    }

    private async Task PublishRequest<T>(string queueName, T notification)
    {
        var json = JsonSerializer.Serialize(notification);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);

        _logger.LogInformation($"[RabbitMQ] Message sent to queue '{queueName}': {json}");
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is { IsOpen: true })
        {
            await _channel.CloseAsync();
            _channel.Dispose();
            await _connection.CloseAsync();
            _connection.Dispose();
        }
    }
}