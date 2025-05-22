namespace EventsService.Application.UseCases.Goals.Commands.CreateGoal;

using AutoMapper;
using EventsService.Application.Contracts;
using EventsService.Application.DTOs.Goals;
using EventsService.Application.DTOs.Notifications;
using EventsService.Domain.Entities;
using MediatR;

public class CreateGoalHandler : IRequestHandler<CreateGoalCommand, GoalDto>
{
    private readonly IGoalRepository _goalRepository;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;

    public CreateGoalHandler(IGoalRepository goalRepository, IMapper mapper, IMessageService messageService)
    {
        this._goalRepository = goalRepository;
        this._mapper = mapper;
        this._messageService = messageService;
    }

    public async Task<GoalDto> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = this._mapper.Map<Goal>(request.Dto);

        goal.Id = Guid.NewGuid();
        var participants = request.Dto.ParticipantIds;
        if (participants.Count > 0)
        {
            foreach (var participantId in participants)
            {
                var notification = new RequestNotification
                {
                    Message = $"You are offered to go to a new goal: {request.Dto.Title}. ",
                    ReceiverId = participantId,
                };

                await this._messageService.PublishGoalRequest(notification);
            }
        }

        if (!goal.ParticipantIds.Contains(request.Dto.Author))
        {
            goal.ParticipantIds.Add(request.Dto.Author);
        }

        await this._goalRepository.CreateAsync(goal, cancellationToken);

        return this._mapper.Map<GoalDto>(goal);
    }
}