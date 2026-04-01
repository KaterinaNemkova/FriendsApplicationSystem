using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Common.Exceptions;
using UserService.Application.Contracts;
using UserService.Application.DTOs.Profiles;

namespace UserService.Application.UseCases.Friends.Queries.GetAllMyFriendsRequests;

public class GetAllMyFriendsRequestsHandler : IRequestHandler<GetAllMyFriendsRequestsQuery, List<ProfileRequestDto>>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IMapper _mapper;
    private readonly IProfileRepository _profileRepository;

    public GetAllMyFriendsRequestsHandler(IFriendshipRepository friendshipRepository, IProfileRepository profileRepository, IMapper mapper)
    {
        _friendshipRepository = friendshipRepository;
        _profileRepository = profileRepository;
        _mapper = mapper;
    }

    public async Task<List<ProfileRequestDto>> Handle(GetAllMyFriendsRequestsQuery request, CancellationToken token)
    {
        var profile = await this._profileRepository.GetByIdAsync(request.ProfileId, token)
                      ?? throw new EntityNotFoundException(nameof(Profile), request.ProfileId);

        var profileIDs = await this._friendshipRepository.GetAllMyFriendsRequestProfileIdsAsync(profile.Id, token);

        var profiles = new List<Domain.Entities.Profile>();
        
        foreach (var profileId in profileIDs)
        {
            Domain.Entities.Profile friendProfile = await this._profileRepository.GetByIdAsync(profileId, token);
            if (friendProfile != null)
            {
                profiles.Add(friendProfile);
            }
        }

        return this._mapper.Map<List<ProfileRequestDto>>(profiles);
    }
}