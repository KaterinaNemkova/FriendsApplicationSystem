namespace UserService.Domain.Entities;

public class Friendship
{
    public Guid Id { get; set; }

    public Guid ProfileId { get; set; } // ID профиля
    public Profile Profile { get; set; }

    public Guid FriendProfileId { get; set; } // ID друга
    public Profile FriendProfile { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
