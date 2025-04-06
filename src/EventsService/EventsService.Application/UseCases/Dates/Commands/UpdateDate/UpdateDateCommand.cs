// <copyright file="UpdateDateCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Dates.Commands.UpdateDate;

using EventsService.Application.DTOs;
using MediatR;

public record UpdateDateCommand(Guid Id, UpdateDateDto DateDto) : IRequest<DateDto>;