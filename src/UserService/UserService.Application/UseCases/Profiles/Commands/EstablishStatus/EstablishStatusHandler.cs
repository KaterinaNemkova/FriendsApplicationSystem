using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Profiles.Commands.EstablishStatus;

public class EstablishStatusHandler:IRequestHandler<EstablishStatusCommand>
{
    private readonly IProfileRepository _profileRepository;

    public EstablishStatusHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }
    
    public async Task Handle(EstablishStatusCommand request, CancellationToken token)
    {
        var profile = await _profileRepository.GetByIdAsync(request.ProfileId, token);
        
        if (profile == null)
            throw new EntityNotFoundException(nameof(profile),request.ProfileId);
        
        await _profileRepository.EstablishStatus(request.ProfileId, request.ActivityStatus, token);
    }
}