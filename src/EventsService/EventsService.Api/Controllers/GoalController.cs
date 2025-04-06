namespace EventsService.Api.Controllers;

using EventsService.Application.DTOs.Goals;
using EventsService.Application.UseCases.Goals.Commands.CreateGoal;
using EventsService.Application.UseCases.Goals.Commands.DeleteGoal;
using EventsService.Application.UseCases.Goals.Commands.UpdateGoal;
using EventsService.Application.UseCases.Goals.Queries.GetAllGoals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

public class GoalController : ControllerBase
{
    private readonly IMediator _mediator;

    public GoalController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal(
        [FromBody] CreateGoalCommand createGoalCommand,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(createGoalCommand, cancellationToken);

        return this.Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGoal([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await this._mediator.Send(new DeleteGoalCommand(id), cancellationToken);

        return this.Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGoal(
        [FromRoute] Guid id,
        [FromBody] UpdateGoalDto updateDateDto,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new UpdateGoalCommand(id, updateDateDto), cancellationToken);
        return this.Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGoals(CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetAllGoalsQuery(), cancellationToken);
        return this.Ok(result);
    }
}