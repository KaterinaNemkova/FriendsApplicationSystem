namespace EventsService.Application.Mappers;

using AutoMapper;
using EventsService.Application.DTOs.Goals;
using EventsService.Domain.Entities;

public class GoalMapper : Profile
{
    public GoalMapper()
    {
        this.CreateMap<Goal, GoalDto>().ReverseMap();
        this.CreateMap<Goal, GoalRequestDto>().ReverseMap();
    }
}