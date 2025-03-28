using AutoMapper;
using UserService.Application.DTOs;


namespace UserService.Application.Mappers;

public class ProfileToProfileDto:Profile
{
    public ProfileToProfileDto()
    {
        CreateMap<Profile, ProfileDto>().ReverseMap();
    }
}