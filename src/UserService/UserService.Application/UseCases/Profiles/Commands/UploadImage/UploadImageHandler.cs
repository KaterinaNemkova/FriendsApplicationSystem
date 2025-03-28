using CloudinaryDotNet.Actions;
using MediatR;
using UserService.Domain.Contracts;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases.Profiles.Commands.UploadImage;

public class UploadImageHandler:IRequestHandler<UploadImageCommand, ImageUploadResult>
{
    private readonly IPhotoService _photoService;
    private readonly IProfileRepository _profileRepository;

    public UploadImageHandler(IPhotoService photoService, IProfileRepository profileRepository)
    {
        _photoService = photoService;
        _profileRepository = profileRepository;
    }
    public async Task<ImageUploadResult> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {

        var profile=await _profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        var result = await _photoService.UploadPhoto(request.File);
        
        if (result.Error != null)
        {
            throw new Exception("Ошибка загрузки фото: " + result.Error.Message);
        }

        var photo = new Photo
        {
            Id=Guid.NewGuid(),
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            ProfileId = profile.Id,

        };
        
        await _profileRepository.UpdatePhotoAsync(request.ProfileId, photo, cancellationToken);
        return result;

    }
}