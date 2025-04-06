// <copyright file="CreateGoalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Goals.Commands.CreateGoal;

using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class CreateGoalHandler : IRequestHandler<CreateGoalCommand, Goal>
{
    private readonly IGoalRepository _goalRepository;

    public CreateGoalHandler(IGoalRepository goalRepository)
    {
        this._goalRepository = goalRepository;
    }

    public async Task<Goal> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = new Goal
        {
            Id = Guid.NewGuid(),
            Description = request.Description,
            Title = request.Title,
            ParticipantIds = request.ParticipantIds,
            TargetDate = request.TargetDate,
            Actions = request.Actions,
        };

        await this._goalRepository.CreateAsync(goal, cancellationToken);
        return goal;
    }
}