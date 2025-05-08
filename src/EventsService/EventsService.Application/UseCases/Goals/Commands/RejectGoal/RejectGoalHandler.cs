namespace EventsService.Application.UseCases.Goals.Commands.RejectGoal;

using EventsService.Application.Contracts;
using MediatR;

public class RejectGoalHandler : IRequestHandler<RejectGoalCommand>
{
    private readonly IGoalRepository _goalRepository;

    public RejectGoalHandler(IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
    }

    // ToDo notificate author that participant reject goal
    public async Task Handle(RejectGoalCommand request, CancellationToken cancellationToken)
    {
        await this._goalRepository.RejectGoalAsync(request.GoalId, request.ProfileId, cancellationToken);
    }
}