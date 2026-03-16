namespace TournamentPlatform.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string? ContactTelegram { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public Guid TournamentId { get; set; }
    public Tournament Tournament { get; set; } = null!;

    public Guid CaptainId { get; set; }
    public User Captain { get; set; } = null!;

    public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}