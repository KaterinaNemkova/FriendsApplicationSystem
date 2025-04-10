namespace EventsService.Domain.Entities;

public class Meeting : Event
{
    public string Address { get; set; } = string.Empty;

    public Guid Author { get; set; }

    public DateTime TimeOfMeet { get; set; }
}