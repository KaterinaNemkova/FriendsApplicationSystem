namespace EventsService.Application.Contracts;

using EventsService.Application.DTOs.Notifications;

public interface IMessageService
{
    Task PublishMeetingRequest(RequestNotification notification);

    Task PublishGoalRequest(RequestNotification notification);

    Task PublishDateRequest(RequestNotification notification);
}