using MediatR;

namespace UserService.Application.UseCases.Friends.Commands.RejectFriendRequest;

public record RejectFriendRequestCommand(Guid ProfileId, Guid FriendProfileId) : IRequest;
