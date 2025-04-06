// <copyright file="UpdateDateDtoToDate.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.Mappers;

using AutoMapper;
using EventsService.Application.DTOs;
using EventsService.Domain.Entities;

public class UpdateDateDtoToDate : Profile
{
    public UpdateDateDtoToDate()
    {
        this.CreateMap<UpdateDateDto, Date>().ReverseMap();
    }
}