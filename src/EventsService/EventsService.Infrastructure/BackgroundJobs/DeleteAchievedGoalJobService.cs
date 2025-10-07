namespace EventsService.Infrastructure.BackgroundJobs;

using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Notifications;
using EventsService.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

public class DeleteAchievedGoalJobService : IDeleteAchievedGoalJobService
{
    private readonly IMongoCollection<Goal> _goalsCollection;
    private readonly IMessageService _messageService;


    public DeleteAchievedGoalJobService(IMongoDatabase database, IMessageService messageService)
    {
        _goalsCollection = database.GetCollection<Goal>("Goals");
        _messageService = messageService;
    }

    public async Task DeleteAchievedGoalsAsync()
    {
        var today = DateTime.UtcNow;
        _goalsCollection.DeleteManyAsync(
            g => g.IsAchieved == true);

        var undefined = await this._goalsCollection.Find(g => g.TargetDate < today && g.IsAchieved == false).ToListAsync();

        foreach (var goal in undefined)
        {
            await SendReminder(goal, $"Do you remember about this goal: {goal.Title}? It should be achieved to {goal.TargetDate}. Change status or target date.");
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
                        Message = $"{message}",
                        ReceiverId = participantId,
                    };

                    await this._messageService.PublishGoalRequest(notificationDto);
                }
            }
    }

}