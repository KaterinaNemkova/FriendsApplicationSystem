namespace EventsService.Domain.Entities;

public abstract class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public List<Guid> ProfilesIds { get; set; } = []; // from UserService

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}