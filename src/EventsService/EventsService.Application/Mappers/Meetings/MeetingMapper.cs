// <copyright file="MeetingToMeetingDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.Mappers;

using AutoMapper;
using EventsService.Application.DTOs.Meetings;
using EventsService.Domain.Entities;

public class MeetingMapper : Profile
{
    public MeetingMapper()
    {
        this.CreateMap<Meeting, MeetingDto>().ReverseMap();
        this.CreateMap<Meeting, MeetingRequestDto>().ReverseMap();
    }
}