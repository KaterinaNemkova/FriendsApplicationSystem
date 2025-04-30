namespace EventsService.Application.UseCases.Dates.Queries.GetAllMyDates;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.Contracts;
using EventsService.Application.DTOs;
using MediatR;

public class GetAllMyDatesHandler : IRequestHandler<GetAllMyDatesQuery, List<DateDto>>
{
    private readonly IDateRepository _dateRepository;
    private readonly IMapper _mapper;

    public GetAllMyDatesHandler(IDateRepository dateRepository, IMapper mapper)
    {
        this._dateRepository = dateRepository;
        this._mapper = mapper;
    }

    public async Task<List<DateDto>> Handle(GetAllMyDatesQuery request, CancellationToken cancellationToken)
    {
        var dates = await this._dateRepository.GetAllMyAsync(request.Id, cancellationToken);
        if (dates == null)
        {
            throw new EntitiesNotFoundException();
        }

        return this._mapper.Map<List<DateDto>>(dates);
    }
}