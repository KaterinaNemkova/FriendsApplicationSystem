using UserService.Application.DTOs;
using UserService.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace UserService.Application.Mappers;

public class FriendshipToFriendshipDto:Profile
{
    public FriendshipToFriendshipDto()
    {
        CreateMap<Friendship, FriendshipDto>().ReverseMap();
    }
}