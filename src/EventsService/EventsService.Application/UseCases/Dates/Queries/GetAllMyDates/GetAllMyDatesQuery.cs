namespace EventsService.Application.UseCases.Dates.Queries.GetAllMyDates;

using EventsService.Application.DTOs;
using MediatR;

public record GetAllMyDatesQuery(Guid Id) : IRequest<List<DateDto>>;
