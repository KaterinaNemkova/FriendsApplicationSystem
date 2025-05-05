namespace UserService.Application.Contracts;

using UserService.Application.DTOs.Notifications;

public interface IMessageService
{
    Task PublishFriendRequest(FriendRequestNotification notification);

    Task PublishProfileCreated(ProfileCreatedNotification notification);
}