namespace TournamentPlatform.Application.DTO.Team;

public class AddTeamMemberDto
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
}