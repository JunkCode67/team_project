using System;
using TournamentPlatform.Domain.Enums;

namespace TournamentPlatform.Application.DTO.Round;

public class RoundResponseDto
{
    public Guid Id { get; set; }
    public Guid TournamentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime Deadline { get; set; }
    public RoundStatus Status { get; set; }
}