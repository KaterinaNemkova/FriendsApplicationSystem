namespace EventsService.Application.Contracts;

using EventsService.Application.DTOs.Notifications;

public interface IMessageService
{
    Task PublishMeetingRequest(MeetingRequestNotification notification);

    Task PublishGoalRequest(GoalRequestNotification notification);
}