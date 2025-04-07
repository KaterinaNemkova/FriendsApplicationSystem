using EventsService.Application.UseCases.Meetings.Queries.GetAllMyMeetings;

namespace EventsService.Api.Controllers;

using EventsService.Application.DTOs.Meetings;
using EventsService.Application.UseCases.Meetings.Commands.CreateMeeting;
using EventsService.Application.UseCases.Meetings.Commands.DeleteMeeting;
using EventsService.Application.UseCases.Meetings.Commands.UpdateMeeting;
using EventsService.Application.UseCases.Meetings.Queries.GetAllFutureMeetings;
using EventsService.Application.UseCases.Meetings.Queries.GetAllMeetings;
using EventsService.Application.UseCases.Meetings.Queries.GetAllPastMeetings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

public class MeetingController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeetingController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMeeting(
        [FromBody] CreateMeetingCommand createMeetingCommand,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(createMeetingCommand, cancellationToken);

        return this.Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeeting([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await this._mediator.Send(new DeleteMeetingCommand(id), cancellationToken);

        return this.Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeeting(
        [FromRoute] Guid id,
        [FromBody] UpdateMeetingDto updateDateDto,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new UpdateMeetingCommand(id, updateDateDto), cancellationToken);
        return this.Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMeetings(CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetAllMeetingsQuery(), cancellationToken);
        return this.Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllMyMeetings([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetAllMyMeetingsQuery(id), cancellationToken);
        return this.Ok(result);
    }

    [HttpGet("future/{id:guid}")]
    public async Task<IActionResult> GetFutureMeetings([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetAllFutureMeetingsQuery(id), cancellationToken);
        return this.Ok(result);
    }

    [HttpGet("past/{id:guid}")]
    public async Task<IActionResult> GetPastMeetings([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetAllPastMeetingsQuery(id), cancellationToken);
        return this.Ok(result);
    }
}