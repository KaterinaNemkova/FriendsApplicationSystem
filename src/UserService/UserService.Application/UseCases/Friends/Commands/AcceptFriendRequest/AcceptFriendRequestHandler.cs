namespace UserService.Application.UseCases.Friends.Commands.AcceptFriendRequest;

using AutoMapper;
using MediatR;
using UserService.Application.Contracts;
using UserService.Application.DTOs.Friendships;
using UserService.Domain.Enums;

public class AcceptFriendRequestHandler : IRequestHandler<AcceptFriendRequestCommand, FriendshipDto>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IMapper _mapper;

    public AcceptFriendRequestHandler(IFriendshipRepository friendshipRepository, IMapper mapper)
    {
        _friendshipRepository = friendshipRepository;
        _mapper = mapper;
    }

    public async Task<FriendshipDto> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var myFriendRequests = await this._friendshipRepository.GetAllMyFriendsRequestsAsync(request.ProfileId, cancellationToken)
            ?? throw new KeyNotFoundException($"Запросы в друзья пользователю {request.ProfileId} не найдены");

        var friendshipToAccept = myFriendRequests
            .FirstOrDefault(f => f.ProfileId == request.FriendProfileId)
            ?? throw new KeyNotFoundException($"Запрос в друзья от пользователя {request.FriendProfileId} не найден");

        friendshipToAccept.RequestStatus = RequestStatus.Accepted;
        friendshipToAccept.BeginningOfInterrelations = DateOnly.FromDateTime(DateTime.UtcNow);

        await this._friendshipRepository
            .AcceptFriendRequestAsync(friendshipToAccept, cancellationToken);

        return this._mapper.Map<FriendshipDto>(friendshipToAccept);
    }
}