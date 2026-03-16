namespace TournamentPlatform.Domain.Entities;

public class Evaluation
{
    public Guid Id { get; set; }
    public int ScoreBackend { get; set; }
    public int ScoreDatabase { get; set; }
    public int ScoreFrontend { get; set; }
    public int ScoreFunctionality { get; set; }
    public int ScoreUsability { get; set; }
    public string? Comment { get; set; }
    public DateTime EvaluatedAt {get;set;} =  DateTime.UtcNow;
    
    public Guid SubmissionId { get; set; }
    public Submission Submission { get; set; } = null;
    
    public Guid JuryId { get; set; }
    public User Jury { get; set; } = null;
}