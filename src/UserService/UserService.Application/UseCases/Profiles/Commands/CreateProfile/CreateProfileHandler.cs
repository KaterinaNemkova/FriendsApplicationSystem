using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Contracts;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using Profile = UserService.Domain.Entities.Profile;

namespace UserService.Application.UseCases.Profiles.Commands.CreateProfile;

public class CreateProfileHandler : IRequestHandler<CreateProfileCommand, ProfileDto>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IMapper _mapper;

    public CreateProfileHandler(IProfileRepository profileRepository, IMapper mapper)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
    }

    public async Task<ProfileDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = new Profile
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Name = request.Name,
            ActivityStatus = ActivityStatus.Busy,
        };

        await _profileRepository.CreateAsync(profile, cancellationToken);

        return _mapper.Map<ProfileDto>(profile);
    }
}