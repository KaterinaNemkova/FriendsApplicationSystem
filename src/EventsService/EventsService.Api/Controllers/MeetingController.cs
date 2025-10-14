using System.IdentityModel.Tokens.Jwt;
using EventsService.Application.UseCases.Meetings.Queries.GetAllMyFutureMeetings;
using EventsService.Application.UseCases.Meetings.Queries.GetAllMyPastMeetings;
using EventsService.Application.Common.Exceptions;

namespace EventsService.Api.Controllers;

using EventsService.Application.DTOs.Meetings;
using EventsService.Application.UseCases.Meetings.Commands.CreateMeeting;
using EventsService.Application.UseCases.Meetings.Commands.DeleteMeeting;
using EventsService.Application.UseCases.Meetings.Commands.UpdateMeeting;
using EventsService.Application.UseCases.Meetings.Queries.GetAllMeetings;
using EventsService.Application.UseCases.Meetings.Queries.GetAllMyMeetings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/meetings")]

public class MeetingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MeetingController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        this._mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMeeting(
        [FromBody] MeetingRequestDto dto,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new CreateMeetingCommand(dto), cancellationToken);
        return this.Ok(result);
    }

    [HttpDelete("{meetingId:guid}")]
    public async Task<IActionResult> DeleteMeeting([FromRoute] Guid meetingId, CancellationToken cancellationToken)
    {
        await this._mediator.Send(new DeleteMeetingCommand(meetingId), cancellationToken);

        return this.Ok();
    }

    [HttpPut("{meetingId:guid}")]
    public async Task<IActionResult> UpdateMeeting(
        [FromRoute] Guid meetingId,
        [FromBody] MeetingRequestDto updateDateDto,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new UpdateMeetingCommand(meetingId, updateDateDto), cancellationToken);

        return this.Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMeetings(CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetAllMeetingsQuery(), cancellationToken);

        return this.Ok(result);
    }

    [HttpGet("my-meetings")]
    public async Task<IActionResult> GetAllMyMeetings(CancellationToken cancellationToken)
    {
        var profileId = GetProfileId();
        var result = await this._mediator.Send(new GetAllMyMeetingsQuery(profileId), cancellationToken);

        return this.Ok(result);
    }

    [HttpGet("my-future-meetings")]
    public async Task<IActionResult> GetFutureMeetings(CancellationToken cancellationToken)
    {
        var profileId = GetProfileId();
        var result = await this._mediator.Send(new GetAllMyFutureMeetingsQuery(profileId), cancellationToken);

        return this.Ok(result);
    }

    [HttpGet("my-past-meetings")]
    public async Task<IActionResult> GetPastMeetings(CancellationToken cancellationToken)
    {
        var profileId = GetProfileId();
        var result = await this._mediator.Send(new GetAllMyPastMeetingsQuery(profileId), cancellationToken);

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