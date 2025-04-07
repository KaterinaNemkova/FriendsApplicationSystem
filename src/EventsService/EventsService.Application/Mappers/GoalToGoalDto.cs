// <copyright file="GoalToGoalDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.Mappers;

using AutoMapper;
using EventsService.Application.DTOs.Goals;
using EventsService.Domain.Entities;

public class GoalToGoalDto : Profile
{
    public GoalToGoalDto()
    {
        this.CreateMap<Goal, GoalDto>().ReverseMap();
    }
}