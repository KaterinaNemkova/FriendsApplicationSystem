// <copyright file="CreateGoalCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Goals.Commands.CreateGoal;

using EventsService.Domain.Entities;
using MediatR;

public record CreateGoalCommand(List<Guid> ParticipantIds, string Title, string Description, DateTime TargetDate, List<string> Actions) : IRequest<Goal>;
