namespace EventsService.Domain.Entities;

public class Goal : Event
{
    public DateTime TargetDate { get; set; }

    public List<string> Actions { get; set; } = [];

    public Guid Author { get; set; }
}