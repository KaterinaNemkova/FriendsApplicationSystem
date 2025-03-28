using System.Windows.Input;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace UserService.Application.UseCases.Profiles.Commands.UploadImage;

public record UploadImageCommand(Guid ProfileId, IFormFile File) : IRequest<ImageUploadResult>;
