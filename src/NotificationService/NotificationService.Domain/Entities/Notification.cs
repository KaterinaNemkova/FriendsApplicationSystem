namespace NotificationService.Domain.Entities;

using NotificationService.Domain.Enums;

public class Notification : Entity
{
    public string Message { get; set; }

    public NotificationType NotificationType { get; set; }

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public DateTime CreatedAt { get; set; }

    public TelegramData? TelegramDetails { get; set; }

    public InAppData? InAppDetails { get; set; }
}