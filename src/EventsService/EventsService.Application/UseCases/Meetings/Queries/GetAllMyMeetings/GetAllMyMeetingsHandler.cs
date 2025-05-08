namespace EventsService.Application.UseCases.Meetings.Queries.GetAllMyMeetings;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Meetings;
using MediatR;

public class GetAllMyMeetingsHandler : IRequestHandler<GetAllMyMeetingsQuery, List<MeetingDto>>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMapper _mapper;

    public GetAllMyMeetingsHandler(IMeetingRepository meetingRepository, IMapper mapper)
    {
        this._meetingRepository = meetingRepository;
        this._mapper = mapper;
    }

    public async Task<List<MeetingDto>> Handle(GetAllMyMeetingsQuery request, CancellationToken cancellationToken)
    {
        var meetings = await this._meetingRepository.GetAllMyAsync(request.Id, cancellationToken);
        if (meetings == null)
        {
            throw new EntitiesNotFoundException();
        }

        return this._mapper.Map<List<MeetingDto>>(meetings);
    }
}