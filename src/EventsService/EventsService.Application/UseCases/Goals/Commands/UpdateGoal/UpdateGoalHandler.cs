namespace EventsService.Application.UseCases.Goals.Commands.UpdateGoal;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.DTOs.Goals;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class UpdateGoalHandler : IRequestHandler<UpdateGoalCommand, GoalDto>
{
    private readonly IGoalRepository _goalRepository;
    private readonly IMapper _mapper;

    public UpdateGoalHandler(IGoalRepository goalRepository, IMapper mapper)
    {
        this._goalRepository = goalRepository;
        this._mapper = mapper;
    }

    public async Task<GoalDto> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
    {
       var goal = await this._goalRepository.GetByIdAsync(request.Id, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(Goal), request.Id);

       var newGoal = this._mapper.Map<Goal>(request.Dto);
       newGoal.Id = request.Id;

       await this._goalRepository.UpdateAsync(newGoal, cancellationToken);

       return this._mapper.Map<GoalDto>(newGoal);
    }
}