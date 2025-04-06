// <copyright file="EntityNotFoundExtension.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.Common.Extensions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string name, object key)
        : base($"Entity {name} with id {key} not found!")
    {
    }
}