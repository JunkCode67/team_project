using System;

namespace TournamentPlatform.Application.DTO.Team;

public class CreateTeamDto
{
    public Guid TournamentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string? ContactTelegram { get; set; }
}