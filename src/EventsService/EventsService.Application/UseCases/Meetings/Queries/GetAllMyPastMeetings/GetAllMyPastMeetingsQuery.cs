namespace EventsService.Application.UseCases.Meetings.Queries.GetAllMyPastMeetings;

using EventsService.Application.DTOs.Meetings;
using MediatR;

public record GetAllMyPastMeetingsQuery(Guid Id) : IRequest<List<MeetingDto>>;
