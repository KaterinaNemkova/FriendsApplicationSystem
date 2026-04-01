using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EventsService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace EventsService.Api.Controllers;

using EventsService.Application.DTOs;
using EventsService.Application.UseCases.Dates.Commands.CreateDate;
using EventsService.Application.UseCases.Dates.Commands.DeleteDate;
using EventsService.Application.UseCases.Dates.Commands.UpdateDate;
using EventsService.Application.UseCases.Dates.Queries.GetAllDates;
using EventsService.Application.UseCases.Dates.Queries.GetAllMyDates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/dates")]
public class DateController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DateController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        this._mediator = mediator;
        this._httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("new")]
    public async Task<IActionResult> CreateDate(
        [FromBody] DateRequestDto dto,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new CreateDateCommand(dto), cancellationToken);

        return this.Ok(result);
    }

    [HttpDelete("{dateId:guid}")]
    public async Task<IActionResult> DeleteDate([FromRoute] Guid dateId, CancellationToken cancellationToken)
    {
        await this._mediator.Send(new DeleteDateCommand(dateId), cancellationToken);

        return this.Ok();
    }

    [HttpPut("{dateId:guid}/changes")]
    public async Task<IActionResult> UpdateDate(
        [FromRoute] Guid dateId,
        [FromBody] DateRequestDto dateRequestDto,
        CancellationToken cancellationToken)
    {
        var date = await this._mediator.Send(new UpdateDateCommand(dateId, dateRequestDto), cancellationToken);
        return this.Ok(date);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDates(CancellationToken cancellationToken)
    {
        var dates = await this._mediator.Send(new GetAllDatesQuery(), cancellationToken);
        return this.Ok(dates);
    }

    [HttpGet("my-dates")]
    public async Task<IActionResult> GetAllMyDates(CancellationToken cancellationToken)
    {
        var profileId = GetProfileId();
        var dates = await this._mediator.Send(new GetAllMyDatesQuery(profileId), cancellationToken);
        return this.Ok(dates);
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