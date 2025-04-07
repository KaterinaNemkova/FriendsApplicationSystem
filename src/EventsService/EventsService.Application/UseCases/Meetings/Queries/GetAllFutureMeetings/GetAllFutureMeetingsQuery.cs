namespace EventsService.Application.UseCases.Meetings.Queries.GetAllFutureMeetings;

using EventsService.Application.DTOs.Meetings;
using MediatR;

public record GetAllFutureMeetingsQuery() : IRequest<List<MeetingDto>>;
