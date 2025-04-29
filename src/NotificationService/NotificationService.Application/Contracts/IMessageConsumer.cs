namespace NotificationService.Application.Contracts;

public interface IMessageConsumer
{
    Task StartConsumingAsync(string queueKey, Func<string, Task> handleMessage, CancellationToken cancellationToken);

    Task InitializeAsync();
}