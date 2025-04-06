// <copyright file="GetAllDatesHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Dates.Queries.GetAllDates;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.DTOs;
using EventsService.Domain.Contracts;
using MediatR;

public class GetAllDatesHandler : IRequestHandler<GetAllDatesQuery, List<DateDto>>
{
    private readonly IDateRepository _dateRepository;
    private readonly IMapper _mapper;

    public GetAllDatesHandler(IDateRepository dateRepository, IMapper mapper)
    {
        this._dateRepository = dateRepository;
        this._mapper = mapper;
    }

    public async Task<List<DateDto>> Handle(GetAllDatesQuery request, CancellationToken cancellationToken)
    {
        var dates = await this._dateRepository.GetAllAsync(cancellationToken);
        if (dates is null)
        {
            throw new EntitiesNotFoundException();
        }

        return this._mapper.Map<List<DateDto>>(dates);
    }
}