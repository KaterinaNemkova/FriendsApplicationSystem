namespace EventsService.Domain.Entities;

public class Date : Event
{
    public int Day { get; set; }

    public int Month { get; set; }

    // Добавляем год (может быть null для ежегодных праздников)
    public int? Year { get; set; }

    // Добавляем тип события (birthday, anniversary, reminder)
    public string Type { get; set; } = "date";
}