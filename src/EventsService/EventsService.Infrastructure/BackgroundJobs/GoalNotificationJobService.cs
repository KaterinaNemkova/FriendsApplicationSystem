namespace EventsService.Infrastructure.BackgroundJobs;

using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Notifications;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class GoalNotificationJobService : IGoalNotificationJobService
{
    private readonly IMongoCollection<Goal> _goalsCollection;
    private readonly IMessageService _messageService;

    public GoalNotificationJobService(IMongoDatabase database, IMessageService messageService)
    {
        _goalsCollection = database.GetCollection<Goal>("Goals");
        _messageService = messageService;
    }

    public async Task CheckFutureGoals()
    {
        var today = DateTime.UtcNow;
        var oneYearLater = today.AddMonths(12);

        var upcomingGoals = await _goalsCollection.Find(
                d => d.TargetDate >= today && d.TargetDate <= oneYearLater)
            .ToListAsync();

        const string reminderTemplate = "You have {0} left before the goal's target day";

        foreach (var goal in upcomingGoals)
        {
            var daysLeft = (goal.TargetDate - today).Days;

            string message = daysLeft switch
                             {
                                 14 => string.Format(reminderTemplate, "2 weeks"),
                                 7 => string.Format(reminderTemplate, "1 week"),
                                 1 => string.Format(reminderTemplate, "1 day"),
                                 0 => string.Format(reminderTemplate, "0 days"),
                                 _ => null,
                             }

                             ?? string.Empty;

            if (message != null)
            {
                await SendReminder(goal, message);
            }
        }
    }

    private async Task SendReminder(Goal goal, string message)
    {
        if (goal.ParticipantIds?.Count > 0)
        {
            foreach (var participantId in goal.ParticipantIds)
            {
                var notificationDto = new RequestNotification
                {
                    Message = $"Notification: {message} - {goal.Title}",
                    ReceiverId = participantId,
                };

                await this._messageService.PublishGoalRequest(notificationDto);
            }
        }
    }
}