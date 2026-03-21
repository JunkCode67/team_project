namespace TournamentPlatform.Application.DTO.Evaluation;

public class LeaderBoardItemDto
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public Guid SubmissionId { get; set; }
    public int AverageScore { get; set; }
    public int TotalEvaluations { get; set; }
    public int ScoreBackend { get; set; }
    public int ScoreDatabase { get; set; }
    public int ScoreFrontend { get; set; }
    public int ScoreFunctionality { get; set; }
    public int ScoreUsability { get; set; }
}