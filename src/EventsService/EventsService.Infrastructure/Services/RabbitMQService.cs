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

    public async Task PublishMeetingRequestAsync(MeetingRequestNotification notification)
    {
        var queueName = this._options.Queues.MeetingRequest;
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

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }
}