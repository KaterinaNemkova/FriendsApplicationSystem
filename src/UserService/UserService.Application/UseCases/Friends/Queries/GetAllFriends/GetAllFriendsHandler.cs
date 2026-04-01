namespace UserService.Application.UseCases.Friends.Queries.GetAllFriends;

using AutoMapper;
using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Application.Contracts;
using UserService.Application.DTOs.Profiles;

public class GetAllFriendsHandler : IRequestHandler<GetAllFriendsQuery, List<ProfileDto>>
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
        var profile = await this._profileRepository.GetByIdAsync(request.ProfileId, token)
            ?? throw new EntityNotFoundException(nameof(Profile), request.ProfileId);

        var friends = await _friendshipRepository.GetAllFriendsAsync(profile.Id, token);

        var result = new List<ProfileDto>();

        foreach (var friend in friends)
        {
            var friendship = await _friendshipRepository.FriendshipExistsByIdsAsync(profile.Id, friend.Id, token);

            var dto = _mapper.Map<ProfileDto>(friend);

            if (friendship != null)
            {
                dto.RelationStatus = friendship.RelationStatus;
                dto.BeginningOfInterrelations = friendship.BeginningOfInterrelations;
            }

            result.Add(dto);
        }

        return result;
    }
}