namespace EventsService.Application.UseCases.Meetings.Commands.UpdateMeeting;

using EventsService.Application.DTOs.Meetings;
using MediatR;

public record UpdateMeetingCommand(Guid Id, UpdateMeetingDto MeetingDto) : IRequest<MeetingDto>;
