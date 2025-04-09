namespace EventsService.Application.UseCases.Meetings.Commands.DeleteMeeting;

using EventsService.Application.Common.Extensions;
using EventsService.Domain.Contracts;
using MediatR;

public class DeleteMeetingHandler : IRequestHandler<DeleteMeetingCommand>
{
    private readonly IMeetingRepository _meetingRepository;

    public DeleteMeetingHandler(IMeetingRepository meetingRepository)
    {
        this._meetingRepository = meetingRepository;
    }

    public async Task Handle(DeleteMeetingCommand request, CancellationToken cancellationToken)
    {
        var meeting = await this._meetingRepository.GetByIdAsync(request.Id, cancellationToken);
        if (meeting == null)
        {
            throw new EntityNotFoundException(nameof(meeting), request.Id);
        }

        await this._meetingRepository.DeleteAsync(request.Id, cancellationToken);
    }
}