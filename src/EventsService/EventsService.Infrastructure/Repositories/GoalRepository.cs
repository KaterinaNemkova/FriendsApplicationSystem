namespace EventsService.Infrastructure.Repositories;

using EventsService.Application.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class GoalRepository : Repository<Goal>, IGoalRepository
{
    public GoalRepository(IMongoCollection<Goal> collection) : base(collection)
    {
    }
}