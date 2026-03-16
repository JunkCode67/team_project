using TournamentPlatform.Domain.Enums;


namespace TournamentPlatform.Domain.Entities;

public class Round
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime Deadline { get; set; }
    public RoundStatus Status { get; set; } = RoundStatus.Draft;

    public Guid TournamentId { get; set; }
    public Tournament Tournament { get; set; } = null!;

    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}