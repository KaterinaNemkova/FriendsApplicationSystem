// <copyright file="GoalRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Infrastructure.Repositories;

using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class GoalRepository : Repository<Goal>, IGoalRepository
{
    public GoalRepository(IMongoCollection<Goal> collection) : base(collection)
    {
    }
}