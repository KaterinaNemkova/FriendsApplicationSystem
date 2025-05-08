namespace UserService.Application.UseCases.Friends.Commands.AddFriend;

using MediatR;
using UserService.Application.DTOs.Friendships;

public record AddFriendCommand(Guid ProfileId, Guid FriendId) : IRequest<FriendshipDto>;