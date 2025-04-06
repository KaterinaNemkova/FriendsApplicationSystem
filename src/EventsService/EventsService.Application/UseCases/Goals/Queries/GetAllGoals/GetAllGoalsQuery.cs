// <copyright file="GetAllGoalsQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Goals.Queries.GetAllGoals;

using EventsService.Application.DTOs.Goals;
using MediatR;

public record GetAllGoalsQuery() : IRequest<List<GoalDto>>;