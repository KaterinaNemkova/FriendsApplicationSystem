using UserService.Domain.Entities;

namespace UserService.Application.UseCases.Profiles.Commands.EstablishStatus;

using MediatR;
using UserService.Application.Common.Exceptions;
using UserService.Application.Contracts;

public class EstablishStatusHandler:IRequestHandler<EstablishStatusCommand>
{
    private readonly IProfileRepository _profileRepository;

    public EstablishStatusHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task Handle(EstablishStatusCommand request, CancellationToken token)
    {
        var profile = await _profileRepository.GetByIdAsync(request.ProfileId, token)
            ?? throw new EntityNotFoundException(nameof(Profile), request.ProfileId);

        await this._profileRepository.EstablishStatus(request.ProfileId, request.ActivityStatus, token);
    }
}