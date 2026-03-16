using System;
using TournamentPlatform.Domain.Enums;

namespace TournamentPlatform.Application.DTO.Submission;

public class SubmissionResponseDto
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    
    // Саме цих полів вам зараз не вистачає:
    public string TeamName { get; set; } = string.Empty;
    public Guid RoundId { get; set; }
    public string GitHubUrl { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public string? Description { get; set; }
    public SubmissionStatus Status { get; set; }
    public DateTime SubmittedAt { get; set; }
}