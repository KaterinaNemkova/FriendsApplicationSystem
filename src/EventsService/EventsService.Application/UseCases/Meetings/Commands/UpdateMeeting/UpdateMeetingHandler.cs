namespace EventsService.Application.UseCases.Meetings.Commands.UpdateMeeting;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.DTOs.Meetings;
using EventsService.Domain.Contracts;
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
        var meeting = await this._meetingRepository.GetByIdAsync(request.Id, cancellationToken);
        if (meeting == null)
        {
            throw new EntityNotFoundException(nameof(meeting), request.Id);
        }

        var newMeeting = new Meeting
        {
            Id = request.Id,
            Title = request.MeetingDto.Title,
            Description = request.MeetingDto.Description,
            ParticipantIds = request.MeetingDto.ParticipantIds,
            Author = request.MeetingDto.Author,
            Address = request.MeetingDto.Address,
            TimeOfMeet = request.MeetingDto.TimeOfMeet,
        };
        await this._meetingRepository.UpdateAsync(newMeeting, cancellationToken);
        return this._mapper.Map<MeetingDto>(newMeeting);
    }
}