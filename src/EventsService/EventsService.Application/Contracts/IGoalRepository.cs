namespace EventsService.Application.Contracts;

using EventsService.Domain.Entities;

public interface IGoalRepository : IRepository<Goal>
{
    Task RejectGoalAsync(Guid goalId, Guid profileId, CancellationToken token);
}