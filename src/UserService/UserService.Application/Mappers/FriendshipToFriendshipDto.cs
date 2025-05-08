namespace UserService.Application.Mappers;

using UserService.Application.DTOs.Friendships;
using UserService.Domain.Entities;
using Profile = AutoMapper.Profile;

public class FriendshipToFriendshipDto : Profile
{
    public FriendshipToFriendshipDto()
    {
        this.CreateMap<Friendship, FriendshipDto>().ReverseMap();
    }
}