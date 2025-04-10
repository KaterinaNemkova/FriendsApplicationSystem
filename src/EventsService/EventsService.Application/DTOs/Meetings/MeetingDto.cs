// <copyright file="MeetingDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.DTOs.Meetings;

public class MeetingDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public List<Guid> ParticipantIds { get; set; }

    public string Address { get; set; }

    public Guid Author { get; set; }

    public DateTime TimeOfMeet { get; set; }
}