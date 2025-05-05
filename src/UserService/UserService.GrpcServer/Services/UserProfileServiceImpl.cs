using Grpc.Core;
using UserService.Application.Contracts;
using UserService.Application.DTOs.Notifications;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.GrpcServer.Services;

public class UserProfileServiceImpl : UserProfileService.UserProfileServiceBase
{
    private readonly IProfileRepository _profileRepository;
    private readonly IMessageService _messageService;

    public UserProfileServiceImpl(IProfileRepository profileRepository, IMessageService messageService)
    {
        _profileRepository = profileRepository;
        _messageService = messageService;
    }

    public override async Task<CreateProfileResponse> CreateProfile(CreateProfileRequest request, ServerCallContext context)
    {
        var profile = new Profile
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(request.UserId),
            Name = request.UserName,
            ActivityStatus = ActivityStatus.Busy,
        };
        await _profileRepository.CreateAsync(profile, context.CancellationToken);
        var profileNotification = new ProfileCreatedNotification
        {
            UserId = profile.UserId,
            ProfileId = profile.Id,
            UserName = profile.Name,
            Message = $"{profile.Name}, your profile with ProfileId: {profile.Id} created"

        };
        await _messageService.PublishProfileCreated(profileNotification);
        
        return new CreateProfileResponse();

    }

    public override async Task<GetUserIdResponse> GetUserId(GetUserIdRequest request, ServerCallContext context)
    {
        var profile = await _profileRepository.GetByIdAsync(Guid.Parse(request.ProfileId), context.CancellationToken);
        return new GetUserIdResponse
        {
            UserId = profile.UserId.ToString()
        };
    }
    
}