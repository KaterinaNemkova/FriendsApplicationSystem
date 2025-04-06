namespace EventsService.Application.DTOs.Meetings;

public class UpdateMeetingDto
{
    public string Title { get; set; }

    public string Description { get; set; }

    public List<Guid> ParticipantIds { get; set; }

    public string Address { get; set; }

    public Guid Author { get; set; }

    public DateTime TimeOfMeet { get; set; }
}