namespace EventsService.Application.UseCases.Meetings.Queries.GetAllMyPastMeetings;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Meetings;
using MediatR;

public class GetAllMyPastMeetingsHandler : IRequestHandler<GetAllMyPastMeetingsQuery, List<MeetingDto>>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMapper _mapper;

    public GetAllMyPastMeetingsHandler(IMeetingRepository meetingRepository, IMapper mapper)
    {
        this._meetingRepository = meetingRepository;
        this._mapper = mapper;
    }

    public async Task<List<MeetingDto>> Handle(GetAllMyPastMeetingsQuery request, CancellationToken cancellationToken)
    {
        var meetings = await this._meetingRepository.GetAllMyAsync(request.Id, cancellationToken);
        if (meetings == null)
        {
            throw new EntitiesNotFoundException();
        }

        var pastMeetings = meetings
            .Where(m => m.TimeOfMeet < DateTime.UtcNow)
            .ToList();

        return this._mapper.Map<List<MeetingDto>>(pastMeetings);
    }
}
