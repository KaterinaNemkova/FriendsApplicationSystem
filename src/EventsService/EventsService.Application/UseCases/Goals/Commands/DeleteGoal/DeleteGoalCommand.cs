// <copyright file="DeleteGoalCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using MediatR;

namespace EventsService.Application.UseCases.Goals.Commands.DeleteGoal;

public record DeleteGoalCommand(Guid Id) : IRequest;
