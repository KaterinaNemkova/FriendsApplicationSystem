// <copyright file="DeleteGoalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Goals.Commands.DeleteGoal;

using EventsService.Application.Common.Extensions;
using EventsService.Domain.Contracts;
using MediatR;

public class DeleteGoalHandler : IRequestHandler<DeleteGoalCommand>
{
    private readonly IGoalRepository _goalRepository;

    public async Task Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
       var goal = await this._goalRepository.GetByIdAsync(request.Id, cancellationToken);
       if (goal == null)
       {
           throw new EntityNotFoundException(nameof(goal), request.Id);
       }

       await this._goalRepository.DeleteAsync(request.Id, cancellationToken);
    }
}