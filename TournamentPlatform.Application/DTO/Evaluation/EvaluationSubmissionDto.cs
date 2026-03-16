using System;
using System.Text.Json.Serialization;

namespace TournamentPlatform.Application.DTO.Evaluation;

public class EvaluateSubmissionDto
{
    [JsonIgnore] 
    public Guid JuryId { get; set; } // Береться з токена в контролері
    
    public Guid SubmissionId { get; set; }
    public int ScoreBackend { get; set; }
    public int ScoreDatabase { get; set; }
    public int ScoreFrontend { get; set; }
    public int ScoreFunctionality { get; set; }
    public int ScoreUsability { get; set; }
    public string? Comment { get; set; }
}