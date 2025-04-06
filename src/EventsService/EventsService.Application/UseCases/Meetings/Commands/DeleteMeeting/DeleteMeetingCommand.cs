namespace EventsService.Application.UseCases.Meetings.Commands.DeleteMeeting;

using MediatR;

public record DeleteMeetingCommand(Guid Id) : IRequest;
