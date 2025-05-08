namespace UserService.Application.DTOs.Notifications;

public class FriendRequestNotification
{
    public Guid SenderUserId { get; set; }

    public Guid ReceiverUserId { get; set; }

    public string Message { get; set; } = string.Empty;
}