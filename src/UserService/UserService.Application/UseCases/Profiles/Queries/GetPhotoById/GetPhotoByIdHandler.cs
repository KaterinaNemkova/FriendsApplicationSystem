using MediatR;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Profiles.Queries.GetPhoto;

public class GetPhotoByIdHandler:IRequestHandler<GetPhotoByIdQuery,string>
{
    private readonly IPhotoService _photoService;
    private readonly IProfileRepository _profileRepository;

    public GetPhotoByIdHandler(IPhotoService photoService, IProfileRepository profileRepository)
    {
        _photoService = photoService;
        _profileRepository = profileRepository;
    }

    public async Task<string> Handle(GetPhotoByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByIdAsync(request.Id, cancellationToken);

        if (profile == null)
            throw new NullReferenceException($"Profile with id {request.Id} not found");

        var url = await _photoService.GetPhoto(profile.Photo.PublicId);
        return url;
    }
}