namespace EventsService.Application.UseCases.Meetings.Commands.CreateMeeting;

using EventsService.Application.DTOs.Meetings;
using MediatR;

public record CreateMeetingCommand(
    string Title,
    string Description,
    List<Guid> ParticipantIds,
    string Address,
    Guid Author,
    DateTime TimeOfMeet) : IRequest<MeetingDto>;
