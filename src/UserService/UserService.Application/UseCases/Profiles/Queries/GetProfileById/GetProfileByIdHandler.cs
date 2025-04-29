namespace UserService.Application.UseCases.Profiles.Queries.GetProfileById;

using AutoMapper;
using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Application.Contracts;
using UserService.Application.DTOs.Profiles;

public class GetProfileByIdHandler(IProfileRepository profileRepository, IMapper mapper)
    : IRequestHandler<GetProfileByIdQuery, ProfileDto>
{
    public async Task<ProfileDto> Handle(GetProfileByIdQuery request, CancellationToken token)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, token)
            ?? throw new EntityNotFoundException(nameof(Profile), request.ProfileId);

        var profileDto = mapper.Map<ProfileDto>(profile);

        return profileDto;
    }
}