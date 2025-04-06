// <copyright file="GetAllDatesQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Dates.Queries.GetAllDates;

using EventsService.Application.DTOs;
using MediatR;

public record GetAllDatesQuery : IRequest<List<DateDto>>;