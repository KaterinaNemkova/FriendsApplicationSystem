using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.UseCases.Profiles.Queries.GetProfileById;

public record GetProfileByIdQuery(Guid ProfileId) : IRequest<ProfileDto>;

