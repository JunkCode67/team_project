using TournamentPlatform.Domain.Enums;

namespace TournamentPlatform.Domain.Entities;

public class Tournament
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TournamentStatus Status { get; set; } = TournamentStatus.Draft;
    public DateTime StartDate { get; set; }
    public DateTime RegistrationStart { get; set; }
    public DateTime RegistrationEnd { get; set; }
    public int? MaxTeams { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Team> Teams { get; set; } = new List<Team>();
    public ICollection<Round> Rounds { get; set; } = new List<Round>();
}