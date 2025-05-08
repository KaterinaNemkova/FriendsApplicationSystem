namespace EventsService.Application.UseCases.Goals.Queries.GetAllMyGoals;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Goals;
using MediatR;

public class GetAllMyGoalsHandler : IRequestHandler<GetAllMyGoalsQuery, List<GoalDto>>
{
    private readonly IGoalRepository _goalRepository;
    private readonly IMapper _mapper;

    public GetAllMyGoalsHandler(IGoalRepository goalRepository, IMapper mapper)
    {
        this._goalRepository = goalRepository;
        this._mapper = mapper;
    }

    public async Task<List<GoalDto>> Handle(GetAllMyGoalsQuery request, CancellationToken cancellationToken)
    {
        var goals = await this._goalRepository.GetAllMyAsync(request.Id, cancellationToken);
        if (goals == null)
        {
            throw new EntitiesNotFoundException();
        }

        return this._mapper.Map<List<GoalDto>>(goals);
    }
}