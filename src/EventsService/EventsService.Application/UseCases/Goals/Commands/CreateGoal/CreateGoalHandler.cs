namespace EventsService.Application.UseCases.Goals.Commands.CreateGoal;

using AutoMapper;
using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Goals;
using EventsService.Domain.Entities;
using MediatR;

public class CreateGoalHandler : IRequestHandler<CreateGoalCommand, GoalDto>
{
    private readonly IGoalRepository _goalRepository;
    private readonly IMapper _mapper;

    public CreateGoalHandler(IGoalRepository goalRepository, IMapper mapper)
    {
        this._goalRepository = goalRepository;
        this._mapper = mapper;
    }

    public async Task<GoalDto> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = this._mapper.Map<Goal>(request.Dto);

        goal.Id = Guid.NewGuid();

        await this._goalRepository.CreateAsync(goal, cancellationToken);

        return this._mapper.Map<GoalDto>(goal);
    }
}