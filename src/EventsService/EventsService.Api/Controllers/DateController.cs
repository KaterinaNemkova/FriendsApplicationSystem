// <copyright file="DateController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using EventsService.Application.DTOs;

namespace EventsService.Api.Controllers;

using EventsService.Application.UseCases.Dates.Commands.CreateDate;
using EventsService.Application.UseCases.Dates.Commands.DeleteDate;
using EventsService.Application.UseCases.Dates.Commands.UpdateDate;
using EventsService.Application.UseCases.Dates.Queries.GetAllDates;
using EventsService.Application.UseCases.Dates.UpdateDate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/controller")]
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDate(
        [FromRoute] Guid id,
        [FromBody] UpdateDateDto updateDateDto,
        CancellationToken cancellationToken)
    {
        var date = await this._mediator.Send(new UpdateDateCommand(id, updateDateDto), cancellationToken);
        return this.Ok(date);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDates(CancellationToken cancellationToken)
    {
        var dates = await this._mediator.Send(new GetAllDatesQuery(), cancellationToken);
        return this.Ok(dates);
    }

}