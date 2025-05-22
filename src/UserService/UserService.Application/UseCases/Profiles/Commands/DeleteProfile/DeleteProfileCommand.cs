namespace UserService.Application.UseCases.Profiles.Commands.DeleteProfile;

using MediatR;

public record DeleteProfileCommand(Guid ProfileId) : IRequest;