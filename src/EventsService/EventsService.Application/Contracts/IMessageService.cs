namespace EventsService.Application.Contracts;

using EventsService.Application.DTOs.Notifications;

public interface IMessageService
{
    Task PublishMeetingRequestAsync(MeetingRequestNotification notification);
}