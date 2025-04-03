// <copyright file="DateToDateDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.Mappers;

using AutoMapper;
using EventsService.Application.DTOs;
using EventsService.Domain.Entities;

public class DateToDateDto : Profile
{
    public DateToDateDto()
    {
        this.CreateMap<Date, DateDto>().ReverseMap();
    }
}