using TournamentPlatform.Domain.Enums;

namespace TournamentPlatform.Domain.Entities;

public class Submission
{
    public Guid Id { get; set; }
    public string GitHubUrl { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string? LiveDemoUrl { get; set; }
    public string? Description { get; set; }

    public SubmissionStatus Status { get; set; } = SubmissionStatus.Draft;
    public DateTime SubmittedAt { get; set; } =  DateTime.UtcNow;
    
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null;
    public Guid RoundId { get; set; }
    public Round Round { get; set; } = null;
    public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
}