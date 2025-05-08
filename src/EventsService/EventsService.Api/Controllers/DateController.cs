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

    public DateController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpPost]
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

    [HttpPut("{dateId:guid}")]
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

    [HttpGet("my-dates/{profileId:guid}")]
    public async Task<IActionResult> GetAllMyDates([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var dates = await this._mediator.Send(new GetAllMyDatesQuery(id), cancellationToken);
        return this.Ok(dates);
    }

}