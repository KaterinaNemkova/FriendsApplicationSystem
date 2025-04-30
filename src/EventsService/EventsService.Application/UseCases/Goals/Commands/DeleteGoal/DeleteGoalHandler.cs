namespace EventsService.Application.UseCases.Goals.Commands.DeleteGoal;

using EventsService.Application.Common.Extensions;
using EventsService.Application.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class DeleteGoalHandler : IRequestHandler<DeleteGoalCommand>
{
    private readonly IGoalRepository _goalRepository;

    public DeleteGoalHandler(IGoalRepository goalRepository)
    {
        this._goalRepository = goalRepository;
    }

    public async Task Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
       var goal = await this._goalRepository.GetByIdAsync(request.Id, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(Goal), request.Id);

       await this._goalRepository.DeleteAsync(request.Id, cancellationToken);
    }
}