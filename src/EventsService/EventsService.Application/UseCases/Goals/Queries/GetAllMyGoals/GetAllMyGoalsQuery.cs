namespace EventsService.Application.UseCases.Goals.Queries.GetAllMyGoals;

using EventsService.Application.DTOs.Goals;
using MediatR;

public record GetAllMyGoalsQuery(Guid Id) : IRequest<List<GoalDto>>;