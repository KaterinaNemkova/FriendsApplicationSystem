// <copyright file="CreateGoalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Goals.Commands.CreateGoal;

using AutoMapper;
using EventsService.Application.DTOs.Goals;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class CreateGoalHandler : IRequestHandler<CreateGoalCommand, GoalDto>
{
    private readonly IGoalRepository _goalRepository;
    private readonly IMapper _mapper;

    public CreateGoalHandler(IGoalRepository goalRepository, IMapper mapper)
    {
        this._goalRepository = goalRepository;
        this._mapper = mapper;
    }

    public async Task<GoalDto> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
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
        return this._mapper.Map<GoalDto>(goal);
    }
}