using System;

namespace TournamentPlatform.Application.DTO.Round;

public class CreateRoundDto
{
    public Guid TournamentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Часові рамки самого раунду
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    // Окремий дедлайн саме для подачі робіт (може збігатися з EndDate, але краще розділяти)
    public DateTime Deadline { get; set; } 
}