namespace NotificationService.Application.DTOs;

using MongoDB.Bson;

public class ProfileCreatedNotification
{
    public ObjectId Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ProfileId { get; set; }

    public string UserName { get; set; }

    public string Message { get; set; } = string.Empty;
}