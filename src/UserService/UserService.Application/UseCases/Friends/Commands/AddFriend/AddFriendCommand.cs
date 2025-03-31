using MediatR;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases.Friends.Commands.AddFriend;

public record AddFriendCommand(Guid ProfileId, Guid FriendId) : IRequest<Friendship>;
