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

    // Інжектимо ті ж самі залежності, що і в інших твоїх сервісах
    public RoundService(UnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<RoundResponseDto> CreateAsync(CreateRoundDto dto)
    {
        // Опціонально: перевіряємо, чи взагалі існує такий турнір
        var tournament = await _context.Tournaments.FindAsync(dto.TournamentId);
        if (tournament == null)
        {
            throw new Exception("Турнір не знайдено");
        }

        // Створюємо нову сутність раунду
        var round = new Round
        {
            Id = Guid.NewGuid(),
            TournamentId = dto.TournamentId,
            Title = dto.Title,
            Description = dto.Description,
            // Переводимо час в UTC, щоб уникнути проблем із часовими поясами в базі
            StartTime = dto.StartDate.ToUniversalTime(),
            Deadline = dto.Deadline.ToUniversalTime(),
            Status = RoundStatus.Draft // За замовчуванням раунд створюється як чернетка
        };

        // Зберігаємо в базу
        await _context.Rounds.AddAsync(round);
        await _context.SaveChangesAsync(); // Або _uow.SaveChangesAsync(), залежно від твоїх налаштувань

        // Повертаємо DTO на фронтенд/Postman
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
        // Шукаємо всі раунди, які належать конкретному турніру
        var rounds = await _context.Rounds
            .Where(r => r.TournamentId == tournamentId)
            .OrderBy(r => r.StartTime) // Сортуємо за датою початку (хронологічно)
            .ToListAsync();

        // Перетворюємо список сутностей у список DTO
        return rounds.Select(MapToDto);
    }

    // Приватний метод-помічник, щоб не писати мапінг 10 разів
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