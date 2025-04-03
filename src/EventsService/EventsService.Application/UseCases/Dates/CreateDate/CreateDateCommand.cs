

using EventsService.Application.DTOs;

namespace EventsService.Application.UseCases.Dates.CreateDate;

using MediatR;

public record CreateDateCommand(string Title, string Description, DateOnly ImportantDate, List<Guid> ParticipantIds) : IRequest<DateDto>;
