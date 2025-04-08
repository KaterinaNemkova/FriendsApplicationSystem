namespace EventsService.Application.UseCases.Goals.Commands.CreateGoal;

using EventsService.Application.DTOs.Goals;
using EventsService.Domain.Entities;
using MediatR;

public record CreateGoalCommand(GoalRequestDto Dto) : IRequest<GoalDto>;
