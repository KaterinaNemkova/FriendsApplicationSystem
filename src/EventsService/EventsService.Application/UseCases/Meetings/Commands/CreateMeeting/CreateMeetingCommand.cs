namespace EventsService.Application.UseCases.Meetings.Commands.CreateMeeting;

using EventsService.Application.DTOs.Meetings;
using MediatR;

public record CreateMeetingCommand(MeetingRequestDto Dto) : IRequest<MeetingDto>;
