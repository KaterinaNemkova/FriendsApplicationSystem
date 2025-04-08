// <copyright file="DateToDateDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.Mappers;

using AutoMapper;
using EventsService.Application.DTOs;
using EventsService.Domain.Entities;

public class DateMapper : Profile
{
    public DateMapper()
    {
        this.CreateMap<Date, DateDto>().ReverseMap();
        this.CreateMap<Date, DateRequestDto>().ReverseMap();
    }
}