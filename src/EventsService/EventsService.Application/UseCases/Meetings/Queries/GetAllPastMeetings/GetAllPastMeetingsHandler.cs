namespace EventsService.Application.UseCases.Meetings.Queries.GetAllPastMeetings;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.DTOs.Meetings;
using EventsService.Domain.Contracts;
using MediatR;

public class GetAllPastMeetingsHandler : IRequestHandler<GetAllPastMeetingsQuery, List<MeetingDto>>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMapper _mapper;

    public GetAllPastMeetingsHandler(IMeetingRepository meetingRepository, IMapper mapper)
    {
        this._meetingRepository = meetingRepository;
        this._mapper = mapper;
    }

    public async Task<List<MeetingDto>> Handle(GetAllPastMeetingsQuery request, CancellationToken cancellationToken)
    {
        var meetings = await this._meetingRepository.GetAllAsync(cancellationToken);
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
