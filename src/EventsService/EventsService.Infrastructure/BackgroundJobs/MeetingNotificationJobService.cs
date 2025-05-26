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
                d => d.TimeOfMeet >= today &&
                     d.TimeOfMeet <= today.AddDays(14))
            .ToListAsync();

        const string reminderTemplate = "You have {0} left before the meeting";

        foreach (var meeting in upcomingMeetings)
        {
            var daysLeft = (meeting.TimeOfMeet - today).Days;

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
                await SendReminder(meeting, message);
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
                    Message = $"Notification: {meeting.Title} - {message}",
                    ReceiverId = participantId,
                };

                await this._messageService.PublishMeetingRequest(notificationDto);
            }
        }
    }
}
