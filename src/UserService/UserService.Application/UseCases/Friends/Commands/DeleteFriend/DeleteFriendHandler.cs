using MediatR;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Friends.Commands.DeleteFriend;

public class DeleteFriendHandler:IRequestHandler<DeleteFriendCommand>
{
    private readonly IFriendshipRepository _friendshipRepository;

    public DeleteFriendHandler(IFriendshipRepository friendshipRepository)
    {
        _friendshipRepository = friendshipRepository;
    }
    public async Task Handle(DeleteFriendCommand request, CancellationToken token)
    {
        var friendship = await _friendshipRepository.FriendshipExistsByIdsAsync(request.ProfileId,request.FriendId,token);
        
        if(friendship == null)
            throw new NullReferenceException("You are not friends");
        
        await _friendshipRepository.DeleteFriendAsync(friendship, token);
    }
}