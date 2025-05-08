namespace NotificationService.Infrastructure.Options;

public class RabbitMQOptions
{
    public string HostName { get; set; }

    public int Port { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public RabbitMQQueues Queues { get; set; } = new();
}

public class RabbitMQQueues
{
    public string FriendRequest { get; set; }

    public string ProfileCreated { get; set; }

    public string MeetingRequest { get; set; }

    public string GoalRequest { get; set; }
}