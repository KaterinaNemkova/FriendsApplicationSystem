namespace EventsService.Application.UseCases.Meetings.Queries.GetAllMyFutureMeetings;

using EventsService.Application.DTOs.Meetings;
using MediatR;

public record GetAllMyFutureMeetingsQuery(Guid Id) : IRequest<List<MeetingDto>>;
