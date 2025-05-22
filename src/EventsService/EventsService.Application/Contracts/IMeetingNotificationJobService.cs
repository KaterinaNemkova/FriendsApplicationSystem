namespace EventsService.Application.Contracts;

public interface IMeetingNotificationJobService
{
    Task CheckFutureMeetings();
}