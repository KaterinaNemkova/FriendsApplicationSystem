namespace EventsService.Application.DTOs;

public class DateRequestDto
{
    public int Day { get; set; }

    public int Month { get; set; }

    public int? Year { get; set; } // Новое поле

    public string Type { get; set; } = "date"; // Новое поле

    public string Title { get; set; }

    public string Description { get; set; }

    public List<Guid> ParticipantIds { get; set; }
}