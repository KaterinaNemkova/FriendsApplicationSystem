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

        await this._meetingRepository.CreateAsync(meeting, cancellationToken);

        if (request.Dto.ParticipantIds?.Count > 0)
        {
            foreach (var participantId in request.Dto.ParticipantIds)
            {
                var notificationDto = new MeetingRequestNotification
                {
                    Message = $"Вас пригласили на встречу: {request.Dto.Title}. ",
                    RecieverId = participantId,
                };

                await this._messageService.PublishMeetingRequestAsync(notificationDto);
            }
        }

        return this._mapper.Map<MeetingDto>(meeting);
    }
}