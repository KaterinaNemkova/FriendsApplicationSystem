using EventsService.Application.DTOs.Goals;

namespace EventsService.Application.UseCases.Goals.Commands.AchieveGoal;

using MediatR;

public record AchieveGoalCommand(Guid GoalId) : IRequest<GoalDto>;