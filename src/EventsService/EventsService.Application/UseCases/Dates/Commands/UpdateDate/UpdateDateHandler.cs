// <copyright file="UpdateDateHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Dates.UpdateDate;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.DTOs;
using EventsService.Application.UseCases.Dates.Commands.UpdateDate;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class UpdateDateHandler : IRequestHandler<UpdateDateCommand, DateDto>
{
    private readonly IDateRepository _dateRepository;
    private readonly IMapper _mapper;

    public UpdateDateHandler(IDateRepository dateRepository, IMapper mapper)
    {
        this._dateRepository = dateRepository;
        this._mapper = mapper;
    }

    public async Task<DateDto> Handle(UpdateDateCommand request, CancellationToken cancellationToken)
    {
        var date = await this._dateRepository.GetByIdAsync(request.Id, cancellationToken);
        if (date == null)
        {
            throw new EntityNotFoundException(nameof(Date), request.Id);
        }

        var newDate = new Date
        {
            Id = request.Id,
            ImportantDate
                = request.DateDto.ImportantDate,
            ParticipantIds = request.DateDto.ParticipantIds,
            Title = request.DateDto.Title,
            Description = request.DateDto.Description,
        };
        await this._dateRepository.UpdateAsync(newDate, cancellationToken);
        return this._mapper.Map<DateDto>(newDate);
    }
}