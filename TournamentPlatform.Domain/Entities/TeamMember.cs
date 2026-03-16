using TournamentPlatform.Domain.Enums;

namespace TournamentPlatform.Domain.Entities;

public class TeamMember
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public TeamRole Role { get; set; } = TeamRole.Member;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}