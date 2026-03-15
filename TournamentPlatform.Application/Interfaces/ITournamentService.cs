using TournamentPlatform.Application.DTOs.Tournament;
using TournamentPlatform.Domain.Enums;

namespace TournamentPlatform.Application.Interfaces;

public interface ITournamentService
{
    Task<TournamentResponseDto> CreateAsync(CreateTournamentDto dto);
    Task<IEnumerable<TournamentResponseDto>> GetAllAsync();
    Task<TournamentResponseDto?> GetByIdAsync(Guid id);
    Task<TournamentResponseDto> UpdateStatusAsync(Guid id, TournamentStatus status);
}