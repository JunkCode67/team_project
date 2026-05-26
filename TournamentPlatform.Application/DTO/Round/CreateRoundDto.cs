using System;

namespace TournamentPlatform.Application.DTO.Round;

public class CreateRoundDto
{
    public Guid TournamentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public DateTime Deadline { get; set; } 
}