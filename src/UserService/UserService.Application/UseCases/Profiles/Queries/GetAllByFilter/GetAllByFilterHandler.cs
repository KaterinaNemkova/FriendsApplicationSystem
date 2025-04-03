using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Contracts;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases.Profiles.Queries.GetAllByFilter;

public class GetAllByFilterHandler:IRequestHandler<GetAllByFilterQuery,List<ProfileDto>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IMapper _mapper;

    public GetAllByFilterHandler(IProfileRepository profileRepository, IMapper mapper)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
    }
    
    public async Task<List<ProfileDto>> Handle(GetAllByFilterQuery request, CancellationToken cancellationToken)
    {
        var profiles=await _profileRepository.GetAllAsync(cancellationToken);
        if (!string.IsNullOrEmpty(request.Name))
        {
            profiles = profiles.Where(p => p.Name.ToLower().Contains(request.Name.ToLower())).ToList();
        }
        
        if (request.ActivityStatus.HasValue)
        {
            profiles = profiles.Where(p => p.ActivityStatus.ToString() == request.ActivityStatus.Value.ToString()).ToList();
        }
        
        if (profiles == null || !profiles.Any())
        {
            throw new KeyNotFoundException("No profiles found for the given filters.");
        }
        
        return _mapper.Map<List<ProfileDto>>(profiles);
        
    }
}