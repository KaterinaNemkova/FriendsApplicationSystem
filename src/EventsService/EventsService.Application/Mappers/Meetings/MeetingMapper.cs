namespace EventsService.Application.Mappers.Meetings;

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