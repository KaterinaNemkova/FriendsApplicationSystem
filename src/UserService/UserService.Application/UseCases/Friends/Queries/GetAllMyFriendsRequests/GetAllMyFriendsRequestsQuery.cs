using MediatR;
using UserService.Application.DTOs.Profiles;

namespace UserService.Application.UseCases.Friends.Queries.GetAllMyFriendsRequests;

public record GetAllMyFriendsRequestsQuery(Guid ProfileId) : IRequest<List<ProfileRequestDto>>;