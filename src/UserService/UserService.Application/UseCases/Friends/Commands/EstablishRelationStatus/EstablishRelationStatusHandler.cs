namespace UserService.Application.UseCases.Friends.Commands.EstablishRelationStatus;

using AutoMapper;
using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Application.DTOs;
using UserService.Domain.Contracts;

public class EstablishRelationStatusHandler(IFriendshipRepository friendshipRepository, IMapper mapper)
    : IRequestHandler<EstablishRelationStatusCommand, FriendshipDto>
{
    public async Task<FriendshipDto> Handle(EstablishRelationStatusCommand request, CancellationToken cancellationToken)
    {
        var friendship = await friendshipRepository.GetFriendshipByIdAsync(request.FriendshipId, cancellationToken);
        if (friendship == null)
        {
            throw new EntityNotFoundException(nameof(friendship), request.FriendshipId);
        }

        var newFriendship = await friendshipRepository.EstablishRelationStatusAsync(friendship, request.Status, cancellationToken);

        return mapper.Map<FriendshipDto>(newFriendship);
    }
}