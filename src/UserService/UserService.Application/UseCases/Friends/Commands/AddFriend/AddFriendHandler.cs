

namespace UserService.Application.UseCases.Friends.Commands.AddFriend;

using AutoMapper;
using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Application.Contracts;
using UserService.Application.DTOs.Friendships;
using UserService.Application.DTOs.Notifications;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

public class AddFriendHandler : IRequestHandler<AddFriendCommand, FriendshipDto>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;

    public AddFriendHandler(
        IFriendshipRepository friendshipRepository,
        IProfileRepository profileRepository,
        IMessageService messageService,
        IMapper mapper)
    {
        this._friendshipRepository = friendshipRepository;
        this._profileRepository = profileRepository;
        this._messageService = messageService;
        this._mapper = mapper;
    }

    public async Task<FriendshipDto> Handle(AddFriendCommand request, CancellationToken token)
    {
        var exist = await this._friendshipRepository.FriendshipExistsByIdsAsync(
            request.ProfileId,
            request.FriendId,
            token);

        if (exist != null && exist.RequestStatus == RequestStatus.Accepted)
        {
            throw new InvalidOperationException("You are already friends");
        }

        if (exist != null && exist.RequestStatus == RequestStatus.Pending)
        {
            throw new InvalidOperationException("You have already sent a request to friends of this person, wait for the answer");
        }

        var profile = await this._profileRepository.GetByIdAsync(request.ProfileId, token)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Profile), request.ProfileId);

        var friend = await this._profileRepository.GetByIdAsync(request.FriendId, token)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Profile), request.FriendId);

        var friendship = new Friendship
        {
            Id = Guid.NewGuid(),
            ProfileId = request.ProfileId,
            Profile = profile,
            FriendProfileId = request.FriendId,
            FriendProfile = friend,
            BeginningOfInterrelations = DateOnly.FromDateTime(DateTime.UtcNow),
            RelationStatus = RelationStatus.Friend,
            RequestStatus = RequestStatus.Pending,
        };

        await this._friendshipRepository.AddFriendAsync(friendship, token);

        var notificationDto = new FriendRequestNotification
        {
            SenderUserId = profile.UserId,
            ReceiverUserId = friend.UserId,
            Message = $"{friend.Name}, you have a new friend request, look at application",
        };

        await this._messageService.PublishFriendRequest(notificationDto);

        return this._mapper.Map<FriendshipDto>(friendship);
    }
}