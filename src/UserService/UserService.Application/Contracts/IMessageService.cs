namespace UserService.Application.Contracts;

using UserService.Application.DTOs.Notifications;

public interface IMessageService
{
    Task PublishFriendRequestAsync(FriendRequestNotification notification);

    Task PublishProfileCreatedAsync(ProfileCreatedNotification notification);
}