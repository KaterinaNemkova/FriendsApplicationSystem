namespace UserService.Application.DTOs.Notifications;

public class ProfileCreatedNotification
{
    public Guid UserId { get; set; }

    public Guid ProfileId { get; set; }

    public string UserName { get; set; }

    public string Message { get; set; } = string.Empty;
}