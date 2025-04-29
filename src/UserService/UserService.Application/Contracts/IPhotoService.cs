namespace UserService.Application.Contracts;

using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

public interface IPhotoService
{
    Task<ImageUploadResult> UploadPhoto(IFormFile file);

    Task<DeletionResult> DeletePhoto(string publicId);

    Task<string> GetPhoto(string publicId);
}