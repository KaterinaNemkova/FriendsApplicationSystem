using CloudinaryDotNet.Actions;
using MediatR;

namespace UserService.Application.UseCases.Profiles.Commands.DeleteImage;

public record DeleteImageCommand(Guid ProfileId) : IRequest<DeletionResult>;