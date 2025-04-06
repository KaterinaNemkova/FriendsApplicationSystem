namespace EventsService.Application.UseCases.Meetings.Queries.GetAllMeetings;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.DTOs.Meetings;
using EventsService.Domain.Contracts;
using MediatR;

public class GetAllMeetingsHandler : IRequestHandler<GetAllMeetingsQuery, List<MeetingDto>>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMapper _mapper;

    public GetAllMeetingsHandler(IMeetingRepository meetingRepository, IMapper mapper)
    {
        this._meetingRepository = meetingRepository;
        this._mapper = mapper;
    }

    public async Task<List<MeetingDto>> Handle(GetAllMeetingsQuery request, CancellationToken cancellationToken)
    {
        var meetings = await this._meetingRepository.GetAllAsync(cancellationToken);
        if (meetings == null)
        {
            throw new EntitiesNotFoundException();
        }

        return this._mapper.Map<List<MeetingDto>>(meetings);
    }
}