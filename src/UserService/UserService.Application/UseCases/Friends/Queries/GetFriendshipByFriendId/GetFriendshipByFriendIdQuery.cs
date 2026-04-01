using MediatR;
using UserService.Application.DTOs.Friendships;

namespace UserService.Application.UseCases.Friends.Queries.GetFriendshipByFriendId;

public record GetFriendshipByFriendIdQuery(Guid ProfileId, Guid FriendId) : IRequest<FriendshipDto>;