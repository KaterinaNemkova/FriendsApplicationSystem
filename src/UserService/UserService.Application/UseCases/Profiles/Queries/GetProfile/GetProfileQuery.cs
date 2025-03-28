using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases.Profiles.Queries.GetProfile;

public record GetProfileQuery(Guid ProfileId) : IRequest<ProfileDto>
{
    
}
