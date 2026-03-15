using TournamentPlatform.Domain.Enums;

namespace TournamentPlatform.Application.DTOs.Tournament;

public class TournamentResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TournamentStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime RegistrationStart { get; set; }
    public DateTime RegistrationEnd { get; set; }
    public int? MaxTeams { get; set; }
    public int TeamsCount { get; set; }
    public DateTime CreatedAt { get; set; }
}