using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Application.Contracts;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases.Profiles.Queries.GetPhoto;

public class GetPhotoByIdHandler:IRequestHandler<GetPhotoByIdQuery, string>
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
        var profile = await _profileRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Profile), request.Id);

        var url = await _photoService.GetPhoto(profile.Photo.PublicId);

        return url;
    }
}