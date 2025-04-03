using CloudinaryDotNet.Actions;
using MediatR;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Profiles.Commands.DeleteImage;

public class DeleteImageHandler: IRequestHandler<DeleteImageCommand, DeletionResult>
{
    private readonly IPhotoService _photoService;
    private readonly IProfileRepository _profileRepository;

    public DeleteImageHandler(IPhotoService photoService,IProfileRepository profileRepository)
    {
        _photoService = photoService;
        _profileRepository = profileRepository;
    }
    
    public async Task<DeletionResult> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        if (profile == null)
            throw new NullReferenceException($"Profile with id {request.ProfileId} not found");

        var deletionResult = await _photoService.DeletePhoto(profile.Photo.PublicId);
        if (deletionResult.Result != "ok")
            throw new ApplicationException("Failed to delete photo from Cloudinary");

        await _profileRepository.DeletePhotoAsync(request.ProfileId, cancellationToken);

        return deletionResult;
    }

}