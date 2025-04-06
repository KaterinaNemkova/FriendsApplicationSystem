// <copyright file="Entity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Domain.Entities;

public class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}