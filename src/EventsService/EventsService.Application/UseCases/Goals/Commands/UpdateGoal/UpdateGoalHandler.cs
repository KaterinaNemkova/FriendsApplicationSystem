// <copyright file="UpdateGoalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Goals.Commands.UpdateGoal;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.DTOs.Goals;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class UpdateGoalHandler : IRequestHandler<UpdateGoalCommand, GoalDto>
{
    private readonly IGoalRepository _goalRepository;
    private readonly IMapper _mapper;

    public UpdateGoalHandler(IGoalRepository goalRepository, IMapper mapper)
    {
        this._goalRepository = goalRepository;
        this._mapper = mapper;
    }

    public async Task<GoalDto> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
    {
       var goal = await this._goalRepository.GetByIdAsync(request.Id, cancellationToken);
       if (goal == null)
       {
           throw new EntityNotFoundException(nameof(goal), request.Id);
       }

       var newGoal = new Goal
       {
           Id = request.Id,
           Title = request.GoalDto.Title,
           Description = request.GoalDto.Description,
           ParticipantIds = request.GoalDto.ParticipantIds,
           TargetDate = request.GoalDto.TargetDate,
           Actions = request.GoalDto.Actions,
       };
       await this._goalRepository.UpdateAsync(newGoal, cancellationToken);
       return this._mapper.Map<GoalDto>(newGoal);
    }
}