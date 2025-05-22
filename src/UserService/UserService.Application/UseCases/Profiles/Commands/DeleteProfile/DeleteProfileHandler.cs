namespace UserService.Application.UseCases.Profiles.Commands.DeleteProfile;

using MediatR;
using UserService.Application.Contracts;

public class DeleteProfileHandler : IRequestHandler<DeleteProfileCommand>
{
    private readonly IProfileRepository _profileRepository;
    private readonly AuthService.GrpcServer.AuthService.AuthServiceClient _authServiceClient;

    public DeleteProfileHandler(IProfileRepository profileRepository, AuthService.GrpcServer.AuthService.AuthServiceClient authService)
    {
        _profileRepository = profileRepository;
        _authServiceClient = authService;
    }

    public async Task Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await this._profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        await this._profileRepository.DeleteAsync(request.ProfileId, cancellationToken);

        var req = new AuthService.GrpcServer.DeleteUserRequest
        {
            UserId = profile.UserId.ToString(),
        };

        var response = await this._authServiceClient.DeleteUserByIdAsync(req);
    }
}