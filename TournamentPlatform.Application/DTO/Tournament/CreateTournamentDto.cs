namespace TournamentPlatform.Application.DTOs.Tournament;

public class CreateTournamentDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime RegistrationStart { get; set; }
    public DateTime RegistrationEnd { get; set; }
    public int? MaxTeams { get; set; }
}