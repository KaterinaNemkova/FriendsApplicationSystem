namespace EventsService.Application.UseCases.Dates.Commands.CreateDate;

using EventsService.Application.DTOs;
using MediatR;

public record CreateDateCommand(DateRequestDto Dto) : IRequest<DateDto>;
