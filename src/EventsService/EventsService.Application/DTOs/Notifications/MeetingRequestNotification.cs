namespace EventsService.Application.DTOs.Notifications;

public class MeetingRequestNotification
{
    public Guid RecieverId { get; set; }

    public string RecieverName { get; set; }

    public string Message { get; set; } = string.Empty;
}