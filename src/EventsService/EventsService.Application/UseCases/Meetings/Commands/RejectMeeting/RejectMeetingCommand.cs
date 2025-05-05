namespace EventsService.Application.UseCases.Meetings.Commands.RejectMeeting;

using MediatR;

public record RejectMeetingCommand(Guid MeetingId, Guid ProfileId) : IRequest;
