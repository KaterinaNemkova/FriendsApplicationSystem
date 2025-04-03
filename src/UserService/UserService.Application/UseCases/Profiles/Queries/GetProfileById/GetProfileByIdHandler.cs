using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Profiles.Queries.GetProfileById;

public class GetProfileByIdHandler(IProfileRepository profileRepository, IMapper mapper)
    : IRequestHandler<GetProfileByIdQuery, ProfileDto>
{
    public async Task<ProfileDto> Handle(GetProfileByIdQuery request, CancellationToken token)
    {
        var profile=await profileRepository.GetByIdAsync(request.ProfileId,token);
        
        if(profile==null)
            throw new NullReferenceException($"Profile with such id {request.ProfileId} was not found");
        
        var profileDto = mapper.Map<ProfileDto>(profile);
        
        return profileDto;
    }
}