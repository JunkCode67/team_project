using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentPlatform.Application.DTO.Round;

namespace TournamentPlatform.Application.Interfaces;

public interface IRoundService
{
    // Створення нового раунду
    Task<RoundResponseDto> CreateAsync(CreateRoundDto dto);
    
    // Отримання конкретного раунду за його ID
    Task<RoundResponseDto?> GetByIdAsync(Guid id);
    
    // Отримання всіх раундів конкретного турніру (щоб вивести їх списком на сторінці турніру)
    Task<IEnumerable<RoundResponseDto>> GetByTournamentIdAsync(Guid tournamentId);
}