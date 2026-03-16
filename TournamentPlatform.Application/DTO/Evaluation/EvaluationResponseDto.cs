using System;

namespace TournamentPlatform.Application.DTO.Evaluation;

public class EvaluationResponseDto
{
    public Guid Id { get; set; }
    public Guid SubmissionId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string JuryName { get; set; } = string.Empty;
    
    public int ScoreBackend { get; set; }
    public int ScoreDatabase { get; set; }
    public int ScoreFrontend { get; set; }
    public int ScoreFunctionality { get; set; }
    public int ScoreUsability { get; set; }
    public int TotalScore { get; set; }
    
    public string? Comment { get; set; }
    public DateTime EvaluatedAt { get; set; }
}