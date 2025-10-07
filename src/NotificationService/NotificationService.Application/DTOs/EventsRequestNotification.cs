namespace NotificationService.Application.DTOs;

public class EventsRequestNotification
{
    public Guid ReceiverId { get; set; }

    public string Message { get; set; } = string.Empty;
}