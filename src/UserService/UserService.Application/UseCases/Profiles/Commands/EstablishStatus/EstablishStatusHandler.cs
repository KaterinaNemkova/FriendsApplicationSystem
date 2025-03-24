using MediatR;
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
        var profile = await _profileRepository.GetByIdAsync(request.Id, token);
        if (profile == null)
        {
            throw new KeyNotFoundException($"Profile with ID {request.Id} not found.");
        }

        await _profileRepository.EstablishStatus(request.Id, request.activityStatus, token);
    }
}