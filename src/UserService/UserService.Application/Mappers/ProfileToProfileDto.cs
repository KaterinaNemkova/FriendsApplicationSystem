namespace UserService.Application.Mappers;

using AutoMapper;
using UserService.Application.DTOs.Profiles;

public class ProfileToProfileDto:Profile
{
    public ProfileToProfileDto()
    {
        this.CreateMap<Domain.Entities.Profile, ProfileDto>();
    }
}