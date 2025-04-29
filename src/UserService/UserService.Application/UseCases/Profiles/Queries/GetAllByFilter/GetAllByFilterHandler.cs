using EventsService.Application.Common.Extensions;

namespace UserService.Application.UseCases.Profiles.Queries.GetAllByFilter;

using AutoMapper;
using MediatR;
using UserService.Application.DTOs.Profiles;
using UserService.Application.Contracts;

public class GetAllByFilterHandler : IRequestHandler<GetAllByFilterQuery, List<ProfileDto>>
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
        var profiles = await this._profileRepository.GetAllAsync(cancellationToken)
                       ?? throw new EntitiesNotFoundException();
        if (!string.IsNullOrEmpty(request.Name))
        {
            profiles = profiles.Where(p => p.Name.ToLower().Contains(request.Name.ToLower())).ToList();
        }

        if (request.ActivityStatus.HasValue)
        {
            profiles = profiles.Where(p => p.ActivityStatus.ToString() == request.ActivityStatus.Value.ToString()).ToList();
        }

        return this._mapper.Map<List<ProfileDto>>(profiles);
    }
}