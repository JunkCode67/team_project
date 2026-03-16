namespace TournamentPlatform.Application.DTO;

public class CreateSubmissionDto
{
    public Guid TeamId { get; set; }
    public Guid RoundId { get; set; }
    public string GitHubUrl { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string? LiveDemoUrl { get; set; }
    public string? Description { get; set; }
}