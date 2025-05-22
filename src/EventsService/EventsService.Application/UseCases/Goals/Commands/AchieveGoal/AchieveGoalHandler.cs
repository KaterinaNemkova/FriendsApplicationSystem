namespace EventsService.Application.UseCases.Goals.Commands.AchieveGoal;

using AutoMapper;
using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Goals;
using MediatR;

public class AchieveGoalHandler : IRequestHandler<AchieveGoalCommand, GoalDto>
{
    private readonly IGoalRepository _goalRepository;
    private readonly IMapper _mapper;

    public AchieveGoalHandler(IGoalRepository goalRepository, IMapper mapper)
    {
        _goalRepository = goalRepository;
        _mapper = mapper;
    }

    public async Task<GoalDto> Handle(AchieveGoalCommand request, CancellationToken cancellationToken)
    {
        var updateGoal = await this._goalRepository.AchieveGoalAsync(request.GoalId, cancellationToken);

        return _mapper.Map<GoalDto>(updateGoal);
    }
}