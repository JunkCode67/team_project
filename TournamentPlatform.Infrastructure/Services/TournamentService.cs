using TournamentPlatform.Application.DTO.Tournament;
using TournamentPlatform.Application.Interfaces;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Domain.Enums;
using TournamentPlatform.Infrastructure.Persistence;

namespace TournamentPlatform.Infrastructure.Services;

public class TournamentService : ITournamentService
{
    private readonly UnitOfWork _uow;

    public TournamentService(UnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TournamentResponseDto> CreateAsync(CreateTournamentDto dto)
    {
        var tournament = new Tournament
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate.ToUniversalTime(),
            RegistrationStart = dto.RegistrationStart.ToUniversalTime(),
            RegistrationEnd = dto.RegistrationEnd.ToUniversalTime(),
            MaxTeams = dto.MaxTeams,
            Status = TournamentStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };

        await _uow.Tournaments.AddAsync(tournament);
        await _uow.SaveChangesAsync();

        return MapToDto(tournament);
    }

    public async Task<IEnumerable<TournamentResponseDto>> GetAllAsync()
    {
        var tournaments = await _uow.Tournaments.GetAllWithTeamsAsync();
        return tournaments.Select(MapToDto);
    }

    public async Task<TournamentResponseDto?> GetByIdAsync(Guid id)
    {
        var tournament = await _uow.Tournaments.GetByIdWithDetailsAsync(id);
        return tournament == null ? null : MapToDto(tournament);
    }

    public async Task<TournamentResponseDto> UpdateStatusAsync(Guid id, TournamentStatus status)
    {
        var tournament = await _uow.Tournaments.GetByIdAsync(id);
        if (tournament == null)
            throw new Exception("Турнір не знайдено");

        tournament.Status = status;
        await _uow.Tournaments.UpdateAsync(tournament);
        await _uow.SaveChangesAsync();

        return MapToDto(tournament);
    }

    private static TournamentResponseDto MapToDto(Tournament t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        Status = t.Status,
        StartDate = t.StartDate,
        RegistrationStart = t.RegistrationStart,
        RegistrationEnd = t.RegistrationEnd,
        MaxTeams = t.MaxTeams,
        TeamsCount = t.Teams?.Count ?? 0,
        CreatedAt = t.CreatedAt
    };
}