
namespace EventsService.Infrastructure.BackgroundJobs;

using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Notifications;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class MeetingNotificationJobService : IMeetingNotificationJobService
{
    private readonly IMongoCollection<Meeting> _meetingsCollection;
    private readonly IMessageService _messageService;

    public MeetingNotificationJobService(IMongoDatabase database, IMessageService messageService)
    {
        _meetingsCollection = database.GetCollection<Meeting>("Meetings");
        _messageService = messageService;
    }

    public async Task CheckFutureMeetings()
    {
        var today = DateTime.UtcNow;

        var upcomingMeetings = await _meetingsCollection.Find(
                d =>
                d.TimeOfMeet >= today &&
                d.TimeOfMeet <= today.AddDays(14))
            .ToListAsync();

        foreach (var meeting in upcomingMeetings)
        {
            var daysLeft = meeting.TimeOfMeet - today;

            if (daysLeft.Days == 14)
            {
                await SendReminder(meeting, "до встречи осталось 2 недели");
            }
            else if (daysLeft.Days == 7)
            {
                await SendReminder(meeting, "до встречи осталась 1 неделя");
            }
            else if (daysLeft.Days == 1)
            {
                await SendReminder(meeting, "завтра у вас назначена встреча!");
            }
            else if (daysLeft.Days == 0)
            {
                await SendReminder(meeting, "сегодня у вас назначена встреча!");
            }
        }
    }

    private async Task SendReminder(Meeting meeting, string message)
    {
        if (meeting.ParticipantIds?.Count > 0)
        {
            foreach (var participantId in meeting.ParticipantIds)
            {
                var notificationDto = new RequestNotification
                {
                    Message = $"Напоминание: {meeting.Title} - {message}",
                    ReceiverId = participantId,
                };

                await this._messageService.PublishMeetingRequest(notificationDto);
            }
        }
    }
}
