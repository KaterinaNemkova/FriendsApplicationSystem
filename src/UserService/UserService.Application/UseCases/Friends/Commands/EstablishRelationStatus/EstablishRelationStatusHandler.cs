using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Friends.Commands.EstablishRelationStatus;

public class EstablishRelationStatusHandler:IRequestHandler<EstablishRelationStatusCommand, FriendshipDto>
{
    private readonly IMapper _mapper;
    private readonly IFriendshipRepository _friendshipRepository;

    public EstablishRelationStatusHandler(IFriendshipRepository friendshipRepository, IMapper mapper)
    {
        _friendshipRepository = friendshipRepository;
        _mapper = mapper;
    }
    public async Task<FriendshipDto> Handle(EstablishRelationStatusCommand request, CancellationToken cancellationToken)
    {
        var friendship=await _friendshipRepository.GetFriendshipByIdAsync(request.FriendshipId, cancellationToken);
        if(friendship == null)
            throw new KeyNotFoundException($"Friendship with id {request.FriendshipId} not found");
        
        var newFriendship=await _friendshipRepository.EstablishRelationStatusAsync(friendship, request.Status, cancellationToken);
        return _mapper.Map<FriendshipDto>(newFriendship);
        
    }
}