namespace EventsService.Api.Controllers;

using EventsService.Application.DTOs;
using EventsService.Application.UseCases.Dates.Commands.CreateDate;
using EventsService.Application.UseCases.Dates.Commands.DeleteDate;
using EventsService.Application.UseCases.Dates.Commands.UpdateDate;
using EventsService.Application.UseCases.Dates.Queries.GetAllDates;
using EventsService.Application.UseCases.Dates.Queries.GetAllMyDates;
using EventsService.Application.UseCases.Dates.UpdateDate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/dates")]
public class DateController : ControllerBase
{
    private readonly IMediator _mediator;

    public DateController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDate(
        [FromBody] CreateDateCommand createDateCommand,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(createDateCommand, cancellationToken);

        return this.Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await this._mediator.Send(new DeleteDateCommand(id), cancellationToken);

        return this.Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateDate(
        [FromRoute] Guid id,
        [FromBody] DateRequestDto dateRequestDto,
        CancellationToken cancellationToken)
    {
        var date = await this._mediator.Send(new UpdateDateCommand(id, dateRequestDto), cancellationToken);
        return this.Ok(date);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDates(CancellationToken cancellationToken)
    {
        var dates = await this._mediator.Send(new GetAllDatesQuery(), cancellationToken);
        return this.Ok(dates);
    }

    [HttpGet("my/{id:guid}")]
    public async Task<IActionResult> GetAllMyDates([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var dates = await this._mediator.Send(new GetAllMyDatesQuery(id), cancellationToken);
        return this.Ok(dates);
    }

}