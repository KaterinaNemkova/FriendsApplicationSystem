// <copyright file="DateController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Api.Controllers;

using EventsService.Application.UseCases.Dates.CreateDate;
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

}