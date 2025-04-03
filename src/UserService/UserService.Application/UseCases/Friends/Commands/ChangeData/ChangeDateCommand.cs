using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases.Friends.Commands.ChangeData;

public record ChangeDateCommand(Guid FriendshipId, DateOnly Date) : IRequest<FriendshipDto>;
