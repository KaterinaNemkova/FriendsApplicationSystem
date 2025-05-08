namespace UserService.Infrastructure.Services;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UserService.Application.Contracts;
using UserService.Infrastructure.Helpers;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecretKey);
        this._cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> UploadPhoto(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face"),
            };
            uploadResult = await this._cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhoto(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var result = await this._cloudinary.DestroyAsync(deletionParams);

        return result;
    }

    public Task<string> GetPhoto(string publicId)
    {
        var photo = this._cloudinary.GetResource(publicId);
        return Task.FromResult(photo.Url);
    }
}