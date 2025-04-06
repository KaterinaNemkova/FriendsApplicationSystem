// <copyright file="UpdateGoalDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.DTOs.Goals;

public class UpdateGoalDto
{
    public string Title { get; set; }

    public string Description { get; set; }

    public List<Guid> ParticipantIds { get; set; }

    public DateTime TargetDate { get; set; }

    public List<string> Actions { get; set; }
}