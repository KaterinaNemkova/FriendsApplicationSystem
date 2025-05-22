namespace EventsService.Domain.Entities;

public class Goal : Event
{
    public DateTimeOffset TargetDate { get; set; }

    public List<string> Actions { get; set; } = [];

    public bool IsAchieved { get; set; } = false;

    public Guid Author { get; set; }
}