namespace EventsService.Application.Mappers.Dates;

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