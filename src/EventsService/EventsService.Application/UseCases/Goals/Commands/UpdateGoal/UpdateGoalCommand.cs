namespace EventsService.Application.UseCases.Goals.Commands.UpdateGoal;

using EventsService.Application.DTOs.Goals;
using MediatR;

public record UpdateGoalCommand(Guid Id, GoalRequestDto Dto) : IRequest<GoalDto>;
