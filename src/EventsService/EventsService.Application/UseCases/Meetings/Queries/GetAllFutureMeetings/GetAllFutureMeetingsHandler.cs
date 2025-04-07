namespace EventsService.Application.UseCases.Meetings.Queries.GetAllFutureMeetings;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.DTOs.Meetings;
using EventsService.Domain.Contracts;
using MediatR;

public class GetAllFutureMeetingsHandler : IRequestHandler<GetAllFutureMeetingsQuery, List<MeetingDto>>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMapper _mapper;

    public GetAllFutureMeetingsHandler(IMeetingRepository meetingRepository, IMapper mapper)
    {
        this._meetingRepository = meetingRepository;
        this._mapper = mapper;
    }

    public async Task<List<MeetingDto>> Handle(GetAllFutureMeetingsQuery request, CancellationToken cancellationToken)
    {
        var meetings = await this._meetingRepository.GetAllAsync(cancellationToken);
        if (meetings == null)
        {
            throw new EntitiesNotFoundException();
        }

        var futureMeetings = meetings
            .Where(m => m.TimeOfMeet > DateTime.UtcNow)
            .ToList();

        return this._mapper.Map<List<MeetingDto>>(futureMeetings);
    }
}