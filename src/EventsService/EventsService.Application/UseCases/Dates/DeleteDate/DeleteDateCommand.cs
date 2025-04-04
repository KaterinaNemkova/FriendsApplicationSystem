// <copyright file="DeleteDateCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.UseCases.Dates.DeleteDate;

using MediatR;

public record DeleteDateCommand(Guid Id) : IRequest;