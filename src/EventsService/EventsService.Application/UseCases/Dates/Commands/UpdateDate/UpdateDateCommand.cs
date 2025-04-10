namespace EventsService.Application.UseCases.Dates.Commands.UpdateDate;

using EventsService.Application.DTOs;
using MediatR;

public record UpdateDateCommand(Guid Id, DateRequestDto Dto) : IRequest<DateDto>;