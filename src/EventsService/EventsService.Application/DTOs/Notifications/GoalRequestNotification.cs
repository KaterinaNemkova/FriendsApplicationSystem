namespace EventsService.Application.DTOs.Notifications;

public class GoalRequestNotification
{
    public Guid ReceiverId { get; set; }

    public string Message { get; set; } = string.Empty;
}