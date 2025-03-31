using MediatR;
using Newtonsoft.Json;

namespace UserService.Application.UseCases.Friends.Commands.DeleteFriend;

public record DeleteFriendCommand(Guid ProfileId, Guid FriendId) : IRequest;
