namespace EventsService.Application.UseCases.Meetings.Queries.GetAllMyMeetings;

using EventsService.Application.DTOs.Meetings;
using MediatR;

public record GetAllMyMeetingsQuery(Guid Id) : IRequest<List<MeetingDto>>;