namespace EventsService.Application.UseCases.Goals.Commands.RejectGoal;

using MediatR;

public record RejectGoalCommand(Guid GoalId, Guid ProfileId) : IRequest;