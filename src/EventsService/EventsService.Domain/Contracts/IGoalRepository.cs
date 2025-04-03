// <copyright file="IGoalRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Domain.Contracts;

using EventsService.Domain.Entities;

public interface IGoalRepository
{
    Task CreateAsync(Goal goal, CancellationToken cancellationToken);
}