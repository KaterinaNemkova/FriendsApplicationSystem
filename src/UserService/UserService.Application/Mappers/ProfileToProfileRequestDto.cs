using UserService.Application.DTOs.Profiles;
using UserService.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace UserService.Application.Mappers;

public class ProfileToProfileRequestDto : Profile
{
    public ProfileToProfileRequestDto()
    {
        this.CreateMap<Domain.Entities.Profile, ProfileRequestDto>();
    }
}