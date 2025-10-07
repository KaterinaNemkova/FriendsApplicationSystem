namespace EventsService.Application.Contracts;

public interface IGoalNotificationJobService
{
    Task CheckFutureGoals();
}