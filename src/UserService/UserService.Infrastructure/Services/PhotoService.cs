using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UserService.Domain.Contracts;
using UserService.Infrastructure.Helpers;

namespace UserService.Infrastructure.Services;

public class PhotoService: IPhotoService
{
    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account
        (
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecretKey
        );
        _cloudinary = new Cloudinary(acc);
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
                Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams); 
            
        }
        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhoto(string PublicId)
    {
        var deletionParams = new DeletionParams(PublicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);
        
        return result;
    }

    public async Task<string> GetPhoto(string PublicId)
    {
        var photo= _cloudinary.GetResource(PublicId);
        return photo.Url;
    }
    
}