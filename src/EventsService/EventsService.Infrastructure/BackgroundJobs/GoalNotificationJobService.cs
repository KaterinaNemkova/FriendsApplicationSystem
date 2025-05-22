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

        foreach (var goal in upcomingGoals)
        {
            var timeLeft = goal.TargetDate - today;
            var totalMonthsLeft = ((goal.TargetDate.Year - today.Year) * 12) +
                goal.TargetDate.Month - today.Month;

            if (timeLeft.Days == 30 * 6)
            {
                await SendReminder(goal, $"До цели осталось 6 месяцев");
            }
            else if (timeLeft.Days == 30 * 3)
            {
                await SendReminder(goal, $"До цели осталось 3 месяца");
            }
            else if (totalMonthsLeft == 1)
            {
                await SendReminder(goal, $"До цели остался 1 месяц");
            }
            else if (timeLeft.Days == 0)
            {
                await SendReminder(goal, $"Сегодня день достижения цели!");
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
                    Message = $"Напоминание: {message} - {goal.Title}",
                    ReceiverId = participantId,
                };

                await this._messageService.PublishGoalRequest(notificationDto);
            }
        }
    }
}