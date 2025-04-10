namespace EventsService.Application.UseCases.Meetings.Commands.CreateMeeting;

using AutoMapper;
using EventsService.Application.DTOs.Meetings;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class CreateMeetingHandler : IRequestHandler<CreateMeetingCommand, MeetingDto>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMapper _mapper;

    public CreateMeetingHandler(IMeetingRepository meetingRepository, IMapper mapper)
    {
        this._meetingRepository = meetingRepository;
        this._mapper = mapper;
    }

    public async Task<MeetingDto> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
    {
        var meeting = this._mapper.Map<Meeting>(request.Dto);
        meeting.Id = Guid.NewGuid();

        await this._meetingRepository.CreateAsync(meeting, cancellationToken);

        return this._mapper.Map<MeetingDto>(meeting);
    }
}