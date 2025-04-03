using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.UseCases.Profiles.Commands.CreateProfile;

public record CreateProfileCommand(Guid UserId, string Name) : IRequest<ProfileDto>;
