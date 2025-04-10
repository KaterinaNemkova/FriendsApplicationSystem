namespace EventsService.Domain.Entities;

public class Date : Event
{
    public DateOnly ImportantDate { get; set; }
}