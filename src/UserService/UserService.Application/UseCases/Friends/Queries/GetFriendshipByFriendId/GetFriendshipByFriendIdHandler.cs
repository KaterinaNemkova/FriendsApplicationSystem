using AutoMapper;
using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Application.Contracts;
using UserService.Application.DTOs.Friendships;

namespace UserService.Application.UseCases.Friends.Queries.GetFriendshipByFriendId;

public class GetFriendshipByFriendIdHandler : IRequestHandler<GetFriendshipByFriendIdQuery, FriendshipDto>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IMapper _mapper;

    public GetFriendshipByFriendIdHandler(IFriendshipRepository friendshipRepository, IMapper mapper)
    {
        _friendshipRepository = friendshipRepository;
        _mapper = mapper;
    }
    public async Task<FriendshipDto> Handle(GetFriendshipByFriendIdQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"=== GetFriendshipByFriendIdHandler ===");
        Console.WriteLine($"ProfileId: {request.ProfileId}");
        Console.WriteLine($"FriendId: {request.FriendId}");
    
        var friendship = await _friendshipRepository.FriendshipExistsByIdsAsync(request.ProfileId, request.FriendId, cancellationToken);
    
        Console.WriteLine($"Friendship found: {friendship != null}");
    
        if (friendship == null)
        {
            Console.WriteLine("Friendship not found - throwing exception");
            throw new EntityNotFoundException(nameof(Domain.Entities.Friendship));
        }
    
        Console.WriteLine($"Friendship ID: {friendship.Id}");
        Console.WriteLine($"Friendship ProfileId: {friendship.ProfileId}");
        Console.WriteLine($"Friendship FriendProfileId: {friendship.FriendProfileId}");
    
        return _mapper.Map<FriendshipDto>(friendship);
    }
}