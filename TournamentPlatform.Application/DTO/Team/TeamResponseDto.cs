using System;

namespace TournamentPlatform.Application.DTO.Team;

public class TeamResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid TournamentId { get; set; }
    public Guid CaptainId { get; set; } // Щоб фронтенд знав, хто лідер
    
    public string Organization { get; set; } = string.Empty;
    public string? ContactTelegram { get; set; }
    public DateTime RegisteredAt { get; set; }
}