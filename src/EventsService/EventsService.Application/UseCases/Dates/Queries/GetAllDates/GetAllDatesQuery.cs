namespace EventsService.Application.UseCases.Dates.Queries.GetAllDates;

using EventsService.Application.DTOs;
using MediatR;

public record GetAllDatesQuery : IRequest<List<DateDto>>;