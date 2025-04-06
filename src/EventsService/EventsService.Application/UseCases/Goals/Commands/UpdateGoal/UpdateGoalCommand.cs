// <copyright file="UpdateGoalCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Goals.Commands.UpdateGoal;

using EventsService.Application.DTOs.Goals;
using MediatR;

public record UpdateGoalCommand(Guid Id, UpdateGoalDto GoalDto) : IRequest<GoalDto>;
