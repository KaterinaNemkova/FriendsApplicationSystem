namespace UserService.Application.UseCases.Friends.Commands.EstablishRelationStatus;

using AutoMapper;
using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Application.Contracts;
using UserService.Application.DTOs.Friendships;
using UserService.Domain.Entities;

public class EstablishRelationStatusHandler(IFriendshipRepository friendshipRepository, IMapper mapper)
    : IRequestHandler<EstablishRelationStatusCommand, FriendshipDto>
{
    public async Task<FriendshipDto> Handle(EstablishRelationStatusCommand request, CancellationToken cancellationToken)
    {
        var friendship = await friendshipRepository.GetFriendshipByIdAsync(request.FriendshipId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Friendship), request.FriendshipId);

        var newFriendship = await friendshipRepository.EstablishRelationStatusAsync(friendship, request.Status, cancellationToken);

        return mapper.Map<FriendshipDto>(newFriendship);
    }
}