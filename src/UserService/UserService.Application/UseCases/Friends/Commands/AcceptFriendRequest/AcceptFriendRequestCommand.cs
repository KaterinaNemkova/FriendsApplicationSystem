namespace UserService.Application.UseCases.Friends.Commands.AcceptFriendRequest;

using MediatR;
using UserService.Application.DTOs.Friendships;

public record AcceptFriendRequestCommand(Guid ProfileId, Guid FriendProfileId) : IRequest<FriendshipDto>;