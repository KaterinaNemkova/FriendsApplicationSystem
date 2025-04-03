using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.UseCases.Friends.Commands.EstablishRelationStatus;

public record EstablishRelationStatusCommand(Guid FriendshipId, RelationStatus Status) : IRequest<FriendshipDto>;
