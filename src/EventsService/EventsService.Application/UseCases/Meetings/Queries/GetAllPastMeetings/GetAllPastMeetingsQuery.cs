namespace EventsService.Application.UseCases.Meetings.Queries.GetAllPastMeetings;

using EventsService.Application.DTOs.Meetings;
using MediatR;

public record GetAllPastMeetingsQuery : IRequest<List<MeetingDto>>;
