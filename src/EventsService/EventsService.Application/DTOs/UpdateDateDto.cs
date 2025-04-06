// <copyright file="UpdateDateDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.DTOs;

public class UpdateDateDto
{
    public DateOnly ImportantDate { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public List<Guid> ParticipantIds { get; set; }
}