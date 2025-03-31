using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Friends.Queries.GetAllFriends;

public class GetAllFriendsHandler:IRequestHandler<GetAllFriendsQuery,List<ProfileDto>>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IMapper _mapper;
    private readonly IProfileRepository _profileRepository;

    public GetAllFriendsHandler(IFriendshipRepository friendshipRepository, IProfileRepository profileRepository, IMapper mapper)
    {
        _friendshipRepository = friendshipRepository;
        _profileRepository = profileRepository;
        _mapper = mapper;
    }
    public async Task<List<ProfileDto>> Handle(GetAllFriendsQuery request, CancellationToken token)
    {
        var profile=await _profileRepository.GetByIdAsync(request.ProfileId,token);
        if(profile==null)
            throw new NullReferenceException("Profile not found");
        
        var profiles=await _friendshipRepository.GetAllFriendsAsync(profile.Id, token);
        return _mapper.Map<List<ProfileDto>>(profiles);
    }
}