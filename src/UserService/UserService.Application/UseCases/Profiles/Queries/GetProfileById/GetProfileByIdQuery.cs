namespace UserService.Application.UseCases.Profiles.Queries.GetProfileById;

using MediatR;
using UserService.Application.DTOs.Profiles;

public record GetProfileByIdQuery(Guid ProfileId) : IRequest<ProfileDto>;

