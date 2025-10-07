namespace EventsService.Application.DTOs;

public class DateDto
{
    public Guid Id { get; set; }

    public int Day { get; set; }

    public int Month { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public List<Guid> ParticipantIds { get; set; }
}