namespace EventsService.Application.UseCases.Meetings.Commands.RejectMeeting;

using EventsService.Application.Contracts;
using MediatR;

public class RejectMeetingHandler : IRequestHandler<RejectMeetingCommand>
{
    private readonly IMeetingRepository _meetingRepository;

    public RejectMeetingHandler(IMeetingRepository meetingRepository)
    {
        _meetingRepository = meetingRepository;
    }

    public async Task Handle(RejectMeetingCommand request, CancellationToken cancellationToken)
    {
        await this._meetingRepository.RejectMeetingAsync(request.MeetingId, request.ProfileId, cancellationToken);
    }
}