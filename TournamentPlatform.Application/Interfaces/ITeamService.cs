using System;
using System.Threading.Tasks;
using TournamentPlatform.Application.DTO.Team;

namespace TournamentPlatform.Application.Interfaces;

public interface ITeamService
{
    // Зверни увагу: ми передаємо captainId окремим параметром, бо беремо його з JWT токена
    Task<TeamResponseDto> RegisterTeamAsync(CreateTeamDto dto, Guid captainId);
}