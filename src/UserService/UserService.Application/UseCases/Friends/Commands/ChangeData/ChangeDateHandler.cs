namespace UserService.Application.UseCases.Friends.Commands.ChangeData;

using AutoMapper;
using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Application.Contracts;
using UserService.Application.DTOs.Friendships;
using UserService.Domain.Entities;

public class ChangeDateHandler:IRequestHandler<ChangeDateCommand, FriendshipDto>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IMapper _mapper;

    public ChangeDateHandler(IFriendshipRepository friendshipRepository, IMapper mapper)
    {
        this._friendshipRepository = friendshipRepository;
        this._mapper = mapper;
    }

    public async Task<FriendshipDto> Handle(ChangeDateCommand request, CancellationToken cancellationToken)
    {
        var friendship = await this._friendshipRepository.GetFriendshipByIdAsync(request.FriendshipId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Friendship), request.FriendshipId);

        var newFriendship = await this._friendshipRepository.ChangeDataOfInterrelations(friendship, request.Date, cancellationToken);

        return this._mapper.Map<FriendshipDto>(newFriendship);
    }
}