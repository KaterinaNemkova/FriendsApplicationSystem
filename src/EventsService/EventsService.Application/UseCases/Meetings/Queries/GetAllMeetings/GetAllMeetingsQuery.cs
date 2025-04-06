namespace EventsService.Application.UseCases.Meetings.Queries.GetAllMeetings;

using EventsService.Application.DTOs.Meetings;
using MediatR;

public record GetAllMeetingsQuery() : IRequest<List<MeetingDto>>;
