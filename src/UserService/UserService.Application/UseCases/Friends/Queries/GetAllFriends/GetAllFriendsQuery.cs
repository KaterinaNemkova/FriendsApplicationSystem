using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.UseCases.Friends.Queries.GetAllFriends;

public record GetAllFriendsQuery(Guid ProfileId):IRequest<List<ProfileDto>>;