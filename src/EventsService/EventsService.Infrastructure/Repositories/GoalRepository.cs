// <copyright file="GoalRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Infrastructure.Repositories;

using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class GoalRepository : IGoalRepository
{
    private readonly IMongoCollection<Goal> _goalsCollection;

    public GoalRepository(IMongoDatabase database)
    {
        _goalsCollection = database.GetCollection<Goal>("Goals");
    }

    public async Task CreateAsync(Goal goal, CancellationToken cancellationToken)
    {
        await _goalsCollection.InsertOneAsync(goal, cancellationToken: cancellationToken);
    }
}