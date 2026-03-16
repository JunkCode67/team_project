using System;

namespace TournamentPlatform.Application.DTO.Evaluation;

public class LeaderboardItemDto
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public double AverageScore { get; set; }
    public int TotalEvaluations { get; set; }
    public double ScoreBackend { get; set; }
    public double ScoreDatabase { get; set; }
    public double ScoreFrontend { get; set; }
    public double ScoreFunctionality { get; set; }
    public double ScoreUsability { get; set; }
}