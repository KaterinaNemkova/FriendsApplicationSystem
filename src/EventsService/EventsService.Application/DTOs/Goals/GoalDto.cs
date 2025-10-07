namespace EventsService.Application.DTOs.Goals;

public class GoalDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public List<Guid> ParticipantIds { get; set; }

    public DateTimeOffset TargetDate { get; set; }

    public List<string> Actions { get; set; }

    public Guid Author { get; set; }

    public bool IsAchieved { get; set; }
}