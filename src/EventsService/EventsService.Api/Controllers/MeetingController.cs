using EventsService.Application.UseCases.Meetings.Queries.GetAllMyFutureMeetings;
using EventsService.Application.UseCases.Meetings.Queries.GetAllMyPastMeetings;
using Microsoft.AspNetCore.Authorization;

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

    public MeetingController(IMediator mediator)
    {
        this._mediator = mediator;
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

    [HttpGet("my-meetings/{profileId:guid}")]
    public async Task<IActionResult> GetAllMyMeetings([FromRoute] Guid profileId, CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetAllMyMeetingsQuery(profileId), cancellationToken);

        return this.Ok(result);
    }

    [HttpGet("my-future-meetings/{profileId:guid}")]
    public async Task<IActionResult> GetFutureMeetings([FromRoute] Guid profileId, CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetAllMyFutureMeetingsQuery(profileId), cancellationToken);

        return this.Ok(result);
    }

    [HttpGet("my-past-meetings/{profileId:guid}")]
    public async Task<IActionResult> GetPastMeetings([FromRoute] Guid profileId, CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetAllMyPastMeetingsQuery(profileId), cancellationToken);

        return this.Ok(result);
    }
}