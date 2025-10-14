using System.IdentityModel.Tokens.Jwt;
using EventsService.Application.Common.Exceptions;
using EventsService.Application.UseCases.Goals.Commands.AchieveGoal;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace EventsService.Api.Controllers;

using EventsService.Application.DTOs.Goals;
using EventsService.Application.UseCases.Goals.Commands.CreateGoal;
using EventsService.Application.UseCases.Goals.Commands.DeleteGoal;
using EventsService.Application.UseCases.Goals.Commands.UpdateGoal;
using EventsService.Application.UseCases.Goals.Queries.GetAllGoals;
using EventsService.Application.UseCases.Goals.Queries.GetAllMyGoals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/goals")]

public class GoalController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GoalController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        this._mediator = mediator;
        this._httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal(
        [FromBody] GoalRequestDto dto,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new CreateGoalCommand(dto), cancellationToken);

        return this.Ok(result);
    }

    [HttpDelete("{goalId:guid}")]
    public async Task<IActionResult> DeleteGoal([FromRoute] Guid goalId, CancellationToken cancellationToken)
    {
        await this._mediator.Send(new DeleteGoalCommand(goalId), cancellationToken);

        return this.Ok();
    }

    [HttpPut("{goalId:guid}")]
    public async Task<IActionResult> UpdateGoal(
        [FromRoute] Guid goalId,
        [FromBody] GoalRequestDto dateRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new UpdateGoalCommand(goalId, dateRequestDto), cancellationToken);

        return this.Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGoals(CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetAllGoalsQuery(), cancellationToken);

        return this.Ok(result);
    }

    [HttpGet("my-goals")]
    public async Task<IActionResult> GetMyGoals(CancellationToken cancellationToken)
    {
        var profileId = GetProfileId();
        var result = await this._mediator.Send(new GetAllMyGoalsQuery(profileId), cancellationToken);

        return this.Ok(result);
    }

    [SwaggerOperation(Summary = "Make done")]
    [HttpPut("done/{goalId:guid}")]

    public async Task<IActionResult> AchieveGoal([FromRoute] Guid goalId, CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new AchieveGoalCommand(goalId), cancellationToken);

        return this.Ok(result);
    }
    
    private Guid GetProfileId()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedException("Unauthorized.", "Token not found in cookies.");

        token = token.Replace("Bearer ", "");

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var profileIdValue = jwtToken.Claims.First(c => c.Type == "profile_id").Value;

        if (!Guid.TryParse(profileIdValue, out var profileId))
            throw new UnauthorizedException("Unauthorized.", "Invalid Profile ID format.");

        return profileId;
    }
}