namespace EventsService.Application.UseCases.Meetings.Commands.CreateMeeting;

using AutoMapper;
using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Meetings;
using EventsService.Application.DTOs.Notifications;
using EventsService.Domain.Entities;
using MediatR;

public class CreateMeetingHandler : IRequestHandler<CreateMeetingCommand, MeetingDto>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;

    public CreateMeetingHandler(IMeetingRepository meetingRepository, IMapper mapper, IMessageService messageService)
    {
        this._meetingRepository = meetingRepository;
        this._mapper = mapper;
        this._messageService = messageService;
    }

    public async Task<MeetingDto> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
    {
        var meeting = this._mapper.Map<Meeting>(request.Dto);
        meeting.Id = Guid.NewGuid();

        if (request.Dto.ParticipantIds?.Count > 0)
        {
            foreach (var participantId in request.Dto.ParticipantIds)
            {
                var notificationDto = new RequestNotification
                {
                    Message = $"You have been invited to the meeting: {request.Dto.Title}. ",
                    ReceiverId = participantId,
                };

                await this._messageService.PublishMeetingRequest(notificationDto);
            }
        }

        if (!meeting.ParticipantIds.Contains(request.Dto.Author))
        {
            meeting.ParticipantIds.Add(request.Dto.Author);
        }

        await this._meetingRepository.CreateAsync(meeting, cancellationToken);

        return this._mapper.Map<MeetingDto>(meeting);
    }
}