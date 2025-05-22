namespace EventsService.Infrastructure.Repositories;

using EventsService.Application.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class GoalRepository : Repository<Goal>, IGoalRepository
{
    public GoalRepository(IMongoCollection<Goal> collection) : base(collection)
    {
    }

    public async Task RejectGoalAsync(Guid goalId, Guid profileId, CancellationToken token)
    {
        var filter = Builders<Goal>.Filter.Eq(p => p.Id, goalId);
        var update = Builders<Goal>.Update.Pull(p => p.ParticipantIds, profileId);
        await this._collection.UpdateOneAsync(filter, update, cancellationToken: token);
    }

    public async Task<Goal> AchieveGoalAsync(Guid id, CancellationToken token)
    {
        var filter = Builders<Goal>.Filter.Eq(p => p.Id, id);
        var update = Builders<Goal>.Update.Set(p => p.IsAchieved, true);

        return await this._collection.FindOneAndUpdateAsync(filter, update, cancellationToken: token);
    }
}