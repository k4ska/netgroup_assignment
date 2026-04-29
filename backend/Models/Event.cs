namespace backend.Models;
public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int MaxParticipants { get; set; }
    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
}