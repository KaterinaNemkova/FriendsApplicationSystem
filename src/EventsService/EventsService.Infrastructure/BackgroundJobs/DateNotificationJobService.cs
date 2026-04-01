namespace EventsService.Infrastructure.BackgroundJobs;

using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Notifications;
using EventsService.Domain.Entities;
using MediatR;
using MongoDB.Driver;

public class DateNotificationJobService : IDateNotificationJobService
{
    private readonly IMongoCollection<Date> _datesCollection;
    private readonly IMessageService _messageService;

    public DateNotificationJobService(IMongoDatabase database, IMessageService messageService)
    {
        _datesCollection = database.GetCollection<Date>("Dates");
        _messageService = messageService;
    }

    public async Task CheckImportantDates()
    {
            var today = DateTime.Today;
            var allDates = await _datesCollection.Find(_ => true).ToListAsync();

            foreach (var date in allDates)
            {
                var dateThisYear = new DateTime(today.Year, date.Month, date.Day);

                if (dateThisYear < today)
                {
                    dateThisYear = dateThisYear.AddYears(1);
                }

                var daysLeft = (dateThisYear - today).Days;

                string eventTypePrefix = date.Type?.ToLower() switch
                {
                    "birthday" => "Birthday 🎉",
                    "anniversary" => "Anniversary 🥂",
                    "reminder" => "Reminder ⏰",
                    _ => "Event"
                };

                switch (daysLeft)
                {
                    case 14:
                        await SendReminder(date, $"{eventTypePrefix}: '{date.Title}' is in 2 weeks ({dateThisYear:dd.MM.yyyy})");
                        break;
                    case 7:
                        await SendReminder(date, $"{eventTypePrefix}: '{date.Title}' is in 1 week ({dateThisYear:dd.MM.yyyy})");
                        break;
                    case 1:
                        await SendReminder(date, $"{eventTypePrefix}: '{date.Title}' is Tomorrow!");
                        break;
                    case 0:
                        await SendReminder(date, $"{eventTypePrefix}: '{date.Title}' is Today! Don't forget!");
                        break;
                }
            }
    }

    private async Task SendReminder(Date date, string message)
    {
        if (date.ParticipantIds?.Count > 0)
        {
            foreach (var participantId in date.ParticipantIds)
            {
                var notificationDto = new RequestNotification
                {
                    Message = $"Notification: {message}",
                    ReceiverId = participantId,
                };

                await this._messageService.PublishDateRequest(notificationDto);
            }
        }
    }
}