// <copyright file="DeleteDateHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using EventsService.Application.Common.Extensions;
using EventsService.Domain.Contracts;
using MediatR;

namespace EventsService.Application.UseCases.Dates.Commands.DeleteDate;

public class DeleteDateHandler:IRequestHandler<DeleteDateCommand>
{
    private readonly IDateRepository _dateRepository;

    public DeleteDateHandler(IDateRepository dateRepository)
    {
        this._dateRepository = dateRepository;
    }

    public async Task Handle(DeleteDateCommand request, CancellationToken cancellationToken)
    {
        var date = await this._dateRepository.GetByIdAsync(request.Id, cancellationToken);
        if (date == null)
        {
            throw new EntityNotFoundException(nameof(date), request.Id);
        }

        await this._dateRepository.DeleteAsync(date.Id, cancellationToken);
    }
}