using MediatR;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases.Friends.Commands.AddFriend;

public record AddFriendCommand: IRequest<Friendship>
{
    public required Guid ProfileId { get; set; }
    public required Guid FriendId { get; set; }
}
