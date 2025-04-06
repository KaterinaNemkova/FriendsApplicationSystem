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
        var meeting = new Meeting
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            ParticipantIds = request.ParticipantIds,
            Author = request.Author,
            Address = request.Address,
            TimeOfMeet = request.TimeOfMeet,
        };
        await this._meetingRepository.CreateAsync(meeting, cancellationToken);

        return this._mapper.Map<MeetingDto>(meeting);
    }
}