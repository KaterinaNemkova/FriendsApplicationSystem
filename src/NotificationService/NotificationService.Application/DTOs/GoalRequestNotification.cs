namespace NotificationService.Application.DTOs;

public class GoalRequestNotification
{
    public Guid ReceiverId { get; set; }

    public string Message { get; set; } = string.Empty;
}