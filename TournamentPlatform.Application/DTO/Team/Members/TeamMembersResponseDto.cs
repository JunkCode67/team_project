namespace TournamentPlatform.Application.DTO.Team;

public class TeamMemberResponseDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; } // Тут буде "Captain" або "Member"
}