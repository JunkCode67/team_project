using System;
using System.Threading.Tasks;
using TournamentPlatform.Application.DTO.Team;

namespace TournamentPlatform.Application.Interfaces;

public interface ITeamService
{
    Task<TeamResponseDto> RegisterTeamAsync(CreateTeamDto dto, Guid captainId);
    Task<IEnumerable<TeamResponseDto>> GetTeamsByTournamentAsync(Guid tournamentId);
    Task AddMemberAsync(AddTeamMemberDto dto, Guid currentUserId);
    Task<IEnumerable<TeamMemberResponseDto>> GetTeamMembersAsync(Guid teamId);
}