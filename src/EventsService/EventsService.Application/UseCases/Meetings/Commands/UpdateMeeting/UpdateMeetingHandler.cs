namespace EventsService.Application.UseCases.Meetings.Commands.UpdateMeeting;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.DTOs.Meetings;
using EventsService.Application.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class UpdateMeetingHandler : IRequestHandler<UpdateMeetingCommand, MeetingDto>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMapper _mapper;

    public UpdateMeetingHandler(IMeetingRepository meetingRepository, IMapper mapper)
    {
        this._meetingRepository = meetingRepository;
        this._mapper = mapper;
    }

    public async Task<MeetingDto> Handle(UpdateMeetingCommand request, CancellationToken cancellationToken)
    {
        var meeting = await this._meetingRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Meeting), request.Id);

        var newMeeting = this._mapper.Map<Meeting>(request.Dto);
        newMeeting.Id = request.Id;
        await this._meetingRepository.UpdateAsync(newMeeting, cancellationToken);

        return this._mapper.Map<MeetingDto>(newMeeting);
    }
}