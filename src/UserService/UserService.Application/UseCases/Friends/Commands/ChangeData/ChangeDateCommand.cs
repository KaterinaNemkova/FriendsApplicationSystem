namespace UserService.Application.UseCases.Friends.Commands.ChangeData;

using MediatR;
using UserService.Application.DTOs.Friendships;

public record ChangeDateCommand(Guid FriendshipId, DateOnly Date) : IRequest<FriendshipDto>;
