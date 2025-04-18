namespace UserService.Application.UseCases.Friends.Commands.AddFriend;

using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Domain.Contracts;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

public class AddFriendHandler : IRequestHandler<AddFriendCommand, Friendship>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly NotificationService.GrpcServer.NotificationService.NotificationServiceClient _notificationClient;

    public AddFriendHandler(
        IFriendshipRepository friendshipRepository,
        IProfileRepository profileRepository,
        NotificationService.GrpcServer.NotificationService.NotificationServiceClient notificationClient)
    {
        this._friendshipRepository = friendshipRepository;
        this._profileRepository = profileRepository;
        this._notificationClient = notificationClient;
    }

    public async Task<Friendship> Handle(AddFriendCommand request, CancellationToken token)
    {
        var exist = await this._friendshipRepository.FriendshipExistsByIdsAsync(request.ProfileId, request.FriendId, token)
            ?? throw new InvalidOperationException("You are already friends");

        var profile = await this._profileRepository.GetByIdAsync(request.ProfileId, token)
            ?? throw new EntityNotFoundException(nameof(Profile), request.ProfileId);

        var friend = await this._profileRepository.GetByIdAsync(request.FriendId, token)
            ?? throw new EntityNotFoundException(nameof(Profile), request.FriendId);

        var friendship = new Friendship
        {
            Id = Guid.NewGuid(),
            ProfileId = request.ProfileId,
            Profile = profile,
            FriendProfileId = request.FriendId,
            FriendProfile = friend,
            BeginningOfInterrelations = DateOnly.FromDateTime(DateTime.UtcNow),
            RelationStatus = RelationStatus.Friend,
        };

        await this._friendshipRepository.AddFriendAsync(friendship, token);

        await this._notificationClient.SendFriendshipNotificationAsync(
            new NotificationService.GrpcServer.FriendshipNotificationRequest()
       {
           ProfileId = request.ProfileId.ToString(),
           FriendProfileId = request.FriendId.ToString(),
           UserId = profile.UserId.ToString(),
           FriendUserId = friend.UserId.ToString(),
       },
            cancellationToken: token);

        return friendship;
    }
}