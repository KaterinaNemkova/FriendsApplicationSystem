namespace NotificationService.Infrastructure.Services.Repositories;

using MongoDB.Driver;
using NotificationService.Domain.Contracts;
using NotificationService.Domain.Entities;

public class NotificationRepository : INotificationRepository
{
    private readonly IMongoCollection<Notification> _notificationsCollection;

    public NotificationRepository(IMongoDatabase database)
    {
        this._notificationsCollection = database.GetCollection<Notification>("Notifications");
    }

    public async Task CreateAsync(Notification notification, CancellationToken token)
    {
        await this._notificationsCollection.InsertOneAsync(notification, token);
    }
}