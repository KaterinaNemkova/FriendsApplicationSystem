namespace EventsService.Application.UseCases.Goals.Commands.DeleteGoal;

using MediatR;

public record DeleteGoalCommand(Guid Id) : IRequest;
