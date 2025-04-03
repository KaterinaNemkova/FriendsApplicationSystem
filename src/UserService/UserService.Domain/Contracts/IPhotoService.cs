using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace UserService.Domain.Contracts;

public interface IPhotoService
{
    Task<ImageUploadResult> UploadPhoto(IFormFile file);
    Task<DeletionResult> DeletePhoto(string PublicId);
    Task<string> GetPhoto(string PublicId);
}