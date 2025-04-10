namespace EventsService.Domain.Entities;

public abstract class Event : Entity
{
    public List<Guid> ParticipantIds { get; set; } = [];

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}