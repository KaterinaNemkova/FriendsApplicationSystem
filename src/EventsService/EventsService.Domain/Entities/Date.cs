namespace EventsService.Domain.Entities;

public class Date : Event
{
    public int Day { get; set; }

    public int Month { get; set; }
}