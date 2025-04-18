namespace NotificationService.Domain.Contracts;

using NotificationService.Domain.Entities;

public interface INotificationRepository
{
    Task CreateAsync(Notification notification, CancellationToken token);
}