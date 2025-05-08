namespace UserService.Application.UseCases.Friends.Commands.RejectFriendRequest;

using MediatR;
using UserService.Application.Contracts;
using UserService.Domain.Enums;

public class RejectFriendRequestHandler : IRequestHandler<RejectFriendRequestCommand>
{
    private readonly IFriendshipRepository _friendshipRepository;

    public RejectFriendRequestHandler(IFriendshipRepository friendshipRepository)
    {
        this._friendshipRepository = friendshipRepository;
    }

    public async Task Handle(RejectFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var myFriendRequests = await this._friendshipRepository.GetAllMyFriendsRequestsAsync(request.ProfileId, cancellationToken);

        var friendshipToReject = myFriendRequests
                                    .FirstOrDefault(f => f.FriendProfileId == request.FriendProfileId)
                                ?? throw new KeyNotFoundException($"Запрос в друзья от пользователя {request.FriendProfileId} не найден");
        
        await this._friendshipRepository.RejectFriendRequestAsync(friendshipToReject.Id, cancellationToken);
    }
}