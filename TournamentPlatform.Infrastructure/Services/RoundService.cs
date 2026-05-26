using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentPlatform.Application.DTO.Round;
using TournamentPlatform.Application.Interfaces;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Domain.Enums;
using TournamentPlatform.Infrastructure.Persistence;

namespace TournamentPlatform.Infrastructure.Services;

public class RoundService : IRoundService
{
    private readonly UnitOfWork _uow;
    private readonly AppDbContext _context;

    public RoundService(UnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<RoundResponseDto> CreateAsync(CreateRoundDto dto)
    {
        var tournament = await _context.Tournaments.FindAsync(dto.TournamentId);
        if (tournament == null)
        {
            throw new Exception("Турнір не знайдено");
        }

        var round = new Round
        {
            Id = Guid.NewGuid(),
            TournamentId = dto.TournamentId,
            Title = dto.Title,
            Description = dto.Description,
            StartTime = dto.StartDate.ToUniversalTime(),
            Deadline = dto.Deadline.ToUniversalTime(),
            Status = RoundStatus.Draft
        };

        // Зберігаємо в базу
        await _context.Rounds.AddAsync(round);
        await _context.SaveChangesAsync();

        return MapToDto(round);
    }

    public async Task<RoundResponseDto?> GetByIdAsync(Guid id)
    {
        var round = await _context.Rounds.FindAsync(id);
        
        if (round == null) return null;

        return MapToDto(round);
    }

    public async Task<IEnumerable<RoundResponseDto>> GetByTournamentIdAsync(Guid tournamentId)
    {
        var rounds = await _context.Rounds
            .Where(r => r.TournamentId == tournamentId)
            .OrderBy(r => r.StartTime) 
            .ToListAsync();

        return rounds.Select(MapToDto);
    }

    private RoundResponseDto MapToDto(Round r)
    {
        return new RoundResponseDto
        {
            Id = r.Id,
            TournamentId = r.TournamentId,
            Title = r.Title,
            Description = r.Description,
            StartDate = r.StartTime,
            EndDate = r.Deadline,
            Deadline = r.Deadline,
            Status = r.Status
        };
    }
}