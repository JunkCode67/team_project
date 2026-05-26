using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentPlatform.Application.DTO.Round;

namespace TournamentPlatform.Application.Interfaces;

public interface IRoundService
{
    Task<RoundResponseDto> CreateAsync(CreateRoundDto dto);
    
    Task<RoundResponseDto?> GetByIdAsync(Guid id);
    
    Task<IEnumerable<RoundResponseDto>> GetByTournamentIdAsync(Guid tournamentId);
}