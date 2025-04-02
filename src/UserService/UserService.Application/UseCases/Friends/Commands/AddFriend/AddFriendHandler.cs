using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Domain.Contracts;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.UseCases.Friends.Commands.AddFriend;

public class AddFriendHandler:IRequestHandler<AddFriendCommand,Friendship>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IProfileRepository _profileRepository;

    public AddFriendHandler(IFriendshipRepository friendshipRepository, IProfileRepository profileRepository)
    {
        _friendshipRepository = friendshipRepository;
        _profileRepository = profileRepository;
    }
        
    public async Task<Friendship> Handle(AddFriendCommand request, CancellationToken token)
    {
        var exist= await _friendshipRepository.FriendshipExistsByIdsAsync(request.ProfileId, request.FriendId, token);
        if(exist!=null)
            throw new InvalidOperationException("You are already friends");
        
        var profile=await _profileRepository.GetByIdAsync(request.ProfileId, token);
        var friend=await _profileRepository.GetByIdAsync(request.FriendId, token);
        
        if(profile==null)
            throw new EntityNotFoundException(nameof(profile), request.ProfileId);
        
        if(friend==null)
            throw new EntityNotFoundException(nameof(friend), request.FriendId);
        
        var friendship = new Friendship
        {
            Id = Guid.NewGuid(),
            ProfileId = request.ProfileId,
            Profile = profile,
            FriendProfileId = request.FriendId,
            FriendProfile = friend,
            BeginningOfInterrelations = DateOnly.FromDateTime(DateTime.UtcNow),
            RelationStatus = RelationStatus.Friend
        };
        
       await _friendshipRepository.AddFriendAsync(friendship, token);
       
       return friendship;
    }
}