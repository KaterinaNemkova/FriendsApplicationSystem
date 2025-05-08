namespace NotificationService.Application.DTOs;

public class FriendRequestNotification
{
    public Guid SenderUserId { get; set; }

    public Guid ReceiverUserId { get; set; }

    public string Message { get; set; } = string.Empty;
}
