namespace EventsService.Application.UseCases.Meetings.Queries.GetAllMyFutureMeetings;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Meetings;
using MediatR;

public class GetAllMyFutureMeetingsHandler : IRequestHandler<GetAllMyFutureMeetingsQuery, List<MeetingDto>>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMapper _mapper;

    public GetAllMyFutureMeetingsHandler(IMeetingRepository meetingRepository, IMapper mapper)
    {
        this._meetingRepository = meetingRepository;
        this._mapper = mapper;
    }

    public async Task<List<MeetingDto>> Handle(GetAllMyFutureMeetingsQuery request, CancellationToken cancellationToken)
    {
        var meetings = await this._meetingRepository.GetAllMyAsync(request.Id, cancellationToken);
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