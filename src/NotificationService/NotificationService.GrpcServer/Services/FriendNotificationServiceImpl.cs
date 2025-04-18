using Grpc.Core;
using NotificationService.Domain.Contracts;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;

namespace NotificationService.GrpcServer.Services;

public class FriendNotificationServiceImpl : NotificationService.NotificationServiceBase
{
    private readonly INotificationRepository _notificationRepository;

    public FriendNotificationServiceImpl(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public override async Task<FriendshipNotificationResponse> SendFriendshipNotification(
        FriendshipNotificationRequest request,
        ServerCallContext context)
    {
        CancellationToken token = context.CancellationToken;
        await _notificationRepository.CreateAsync(new Notification
        {
            Id = Guid.NewGuid(),
            NotificationType = NotificationType.FriendRequest,
            SenderId = Guid.Parse(request.ProfileId),
            ReceiverId = Guid.Parse(request.FriendProfileId),
            Message = $"У вас новый друг!",
            CreatedAt = DateTime.UtcNow,
            
        }, token );
        
        return new FriendshipNotificationResponse { Success = true };
    }
}