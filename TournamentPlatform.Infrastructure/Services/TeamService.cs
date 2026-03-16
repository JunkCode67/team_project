using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentPlatform.Application.DTO.Team;
using TournamentPlatform.Application.Interfaces;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Domain.Enums;
using TournamentPlatform.Infrastructure.Persistence;

namespace TournamentPlatform.Infrastructure.Services;

public class TeamService : ITeamService
{
    private readonly UnitOfWork _uow;
    private readonly AppDbContext _context;

    public TeamService(UnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<TeamResponseDto> RegisterTeamAsync(CreateTeamDto dto, Guid captainId)
    {
        // 1. Перевіряємо, чи існує турнір
        var tournament = await _context.Tournaments.FindAsync(dto.TournamentId);
        if (tournament == null)
            throw new Exception("Турнір не знайдено");

        // 2. Перевіряємо унікальність назви команди в межах турніру
        var existingTeam = await _context.Teams
            .FirstOrDefaultAsync(t => t.Name == dto.Name && t.TournamentId == dto.TournamentId);
        if (existingTeam != null)
            throw new Exception("Команда з такою назвою вже зареєстрована на цей турнір");

        // 3. Створюємо команду, використовуючи всі твої поля
        var team = new Team
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Organization = dto.Organization,
            ContactTelegram = dto.ContactTelegram,
            TournamentId = dto.TournamentId,
            CaptainId = captainId
        };

        await _context.Teams.AddAsync(team);

        // 4. Одразу додаємо капітана як учасника команди 
        // (Припускаю, що у TeamMember є поля TeamId та UserId)
        var teamMember = new TeamMember
        {
            Id = Guid.NewGuid(),
            TeamId = team.Id,
            UserId = captainId,
            Role = TeamRole.Captain
        };

        await _context.TeamMembers.AddAsync(teamMember);

        // 5. Зберігаємо зміни
        await _context.SaveChangesAsync();

        // 6. Повертаємо результат
        return new TeamResponseDto
        {
            Id = team.Id,
            Name = team.Name,
            Organization = team.Organization,
            ContactTelegram = team.ContactTelegram,
            TournamentId = team.TournamentId,
            CaptainId = team.CaptainId,
            RegisteredAt = team.RegisteredAt
        };
    }
}