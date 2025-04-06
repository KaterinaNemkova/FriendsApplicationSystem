namespace EventsService.Domain.Entities;

public class Goal : Event
{
    public DateTime TargetDate { get; set; }

    public List<string> Actions { get; set; } = []; // list of actions that need to do for target goal
}