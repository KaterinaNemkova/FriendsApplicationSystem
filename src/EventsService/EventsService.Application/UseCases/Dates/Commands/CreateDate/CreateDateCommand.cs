namespace EventsService.Application.UseCases.Dates.Commands.CreateDate;

using EventsService.Application.DTOs;
using MediatR;

public record CreateDateCommand(string Title, string Description, DateOnly ImportantDate, List<Guid> ParticipantIds) : IRequest<DateDto>;
