namespace UserService.Application.UseCases.Friends.Commands.EstablishRelationStatus;

using MediatR;
using UserService.Application.DTOs.Friendships;
using UserService.Domain.Enums;

public record EstablishRelationStatusCommand(Guid FriendshipId, RelationStatus Status) : IRequest<FriendshipDto>;
