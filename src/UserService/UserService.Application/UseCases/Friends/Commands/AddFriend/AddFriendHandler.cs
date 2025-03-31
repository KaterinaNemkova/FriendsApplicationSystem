using MediatR;
using UserService.Domain.Contracts;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.UseCases.Friends.Commands.AddFriend;

public class AddFriendHandler:IRequestHandler<AddFriendCommand,Friendship>
{
    private readonly IFriendshipRepository _friendshipRepository;

    public AddFriendHandler(IFriendshipRepository friendshipRepository)
    {
        _friendshipRepository = friendshipRepository;
    }
        
    public async Task<Friendship> Handle(AddFriendCommand request, CancellationToken token)
    {
        var exist= await _friendshipRepository.FriendshipExistsAsync(request.ProfileId, request.FriendId, token);
        
        if(exist)
            throw new InvalidOperationException("You are already friends");
        
        var friendship = new Friendship
        {
            Id = Guid.NewGuid(),
            ProfileId = request.ProfileId,
            FriendProfileId = request.FriendId,
            AddedAt = DateTime.UtcNow,
            RelationStatus = RelationStatus.Friend
        };
        
       await _friendshipRepository.AddFriendAsync(friendship, token);
       
       return friendship;
    }
}