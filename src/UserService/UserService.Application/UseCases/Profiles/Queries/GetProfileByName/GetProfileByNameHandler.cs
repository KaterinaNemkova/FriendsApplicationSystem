using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Profiles.Queries.GetProfileByName;

public class GetProfileByNameHandler:IRequestHandler<GetProfileByNameQuery,ProfileDto>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IMapper _mapper;

    public GetProfileByNameHandler(IProfileRepository profileRepository, IMapper mapper)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
    }
    public async Task<ProfileDto> Handle(GetProfileByNameQuery request, CancellationToken token)
    {
        var profile = await _profileRepository.GetByNameAsync(request.Name, token);
        
        if(profile == null)
            throw new KeyNotFoundException($"Profile with name {request.Name} not found");
        
        return _mapper.Map<ProfileDto>(profile);
    }
}