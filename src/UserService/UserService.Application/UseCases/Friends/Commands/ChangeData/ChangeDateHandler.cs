using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Friends.Commands.ChangeData;

public class ChangeDateHandler:IRequestHandler<ChangeDateCommand,FriendshipDto>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IMapper _mapper;

    public ChangeDateHandler(IFriendshipRepository friendshipRepository,IMapper mapper)
    {
        _friendshipRepository = friendshipRepository;
        _mapper = mapper;
    }
    public async Task<FriendshipDto> Handle(ChangeDateCommand request, CancellationToken cancellationToken)
    {
        var friendship=await _friendshipRepository.GetFriendshipByIdAsync(request.FriendshipId, cancellationToken);
        if(friendship == null)
            throw new KeyNotFoundException($"Friendship with id {request.FriendshipId} not found");
        
        var newFriendship=await _friendshipRepository.ChangeDataOfInterrelations(friendship, request.Date, cancellationToken);
        return _mapper.Map<FriendshipDto>(newFriendship);
    }
}