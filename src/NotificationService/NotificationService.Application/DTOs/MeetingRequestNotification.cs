namespace NotificationService.Application.DTOs;

public class MeetingRequestNotification
{
    public Guid ReceiverId { get; set; }

    public string Message { get; set; } = string.Empty;
}