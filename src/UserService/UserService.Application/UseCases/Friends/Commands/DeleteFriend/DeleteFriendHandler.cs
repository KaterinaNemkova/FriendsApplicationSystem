namespace UserService.Application.UseCases.Friends.Commands.DeleteFriend;

using MediatR;
using UserService.Application.Contracts;

public class DeleteFriendHandler:IRequestHandler<DeleteFriendCommand>
{
    private readonly IFriendshipRepository _friendshipRepository;

    public DeleteFriendHandler(IFriendshipRepository friendshipRepository)
    {
        this._friendshipRepository = friendshipRepository;
    }

    public async Task Handle(DeleteFriendCommand request, CancellationToken token)
    {
        var friendship = await _friendshipRepository.FriendshipExistsByIdsAsync(request.ProfileId,request.FriendId,token);

        if (friendship == null)
        {
            throw new NullReferenceException("You are not friends");
        }

        await this._friendshipRepository.DeleteFriendAsync(friendship, token);
    }
}