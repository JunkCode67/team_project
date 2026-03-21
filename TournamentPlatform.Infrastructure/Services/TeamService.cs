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
    public async Task<IEnumerable<TeamMemberResponseDto>> GetTeamMembersAsync(Guid teamId)
    {
        var members = await _context.TeamMembers
            .Where(tm => tm.TeamId == teamId)
            .Join(_context.Users, 
                tm => tm.UserId,            // Ключ з таблиці TeamMembers
                u => u.Id,                  // Ключ з таблиці Users
                (tm, u) => new TeamMemberResponseDto // Що збираємо на виході
                {
                    UserId = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = tm.Role.ToString()
                })
            .ToListAsync();

        return members;
    }
    public async Task AddMemberAsync(AddTeamMemberDto dto, Guid currentUserId)
    {
        // 1. Шукаємо команду
        var team = await _context.Teams.FindAsync(dto.TeamId);
        if (team == null)
            throw new Exception("Команду не знайдено");

        // 2. СУВОРА ПЕРЕВІРКА: Чи той, хто робить запит — це капітан?
        if (team.CaptainId != currentUserId)
            throw new Exception("Тільки капітан команди може додавати нових учасників");

        // 3. Перевіряємо, чи цей користувач вже не є в команді
        var userAlreadyInTeam = await _context.TeamMembers
            .AnyAsync(tm => tm.TeamId == dto.TeamId && tm.UserId == dto.UserId);
        
        if (userAlreadyInTeam)
            throw new Exception("Цей користувач вже є учасником команди");

        // 4. Якщо всі перевірки пройдені — додаємо учасника
        var teamMember = new TeamMember
        {
            Id = Guid.NewGuid(),
            TeamId = dto.TeamId,
            UserId = dto.UserId,
            Role = TeamRole.Member // Звичайна роль учасника
        };

        await _context.TeamMembers.AddAsync(teamMember);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<TeamResponseDto>> GetTeamsByTournamentAsync(Guid tournamentId)
    {
        var teams = await _context.Teams
            .Where(t => t.TournamentId == tournamentId)
            .ToListAsync();

        return teams.Select(t => new TeamResponseDto
        {
            Id = t.Id,
            Name = t.Name,
            Organization = t.Organization,
            ContactTelegram = t.ContactTelegram,
            TournamentId = t.TournamentId,
            CaptainId = t.CaptainId,
            RegisteredAt = t.RegisteredAt
        });
    }

    public async Task<TeamResponseDto> RegisterTeamAsync(CreateTeamDto dto, Guid captainId)
    {
        // 1. Перевіряємо, чи існує турнір
        var tournament = await _context.Tournaments.FindAsync(dto.TournamentId);
        if (tournament == null)
            throw new Exception("Турнір не знайдено");
        var now = DateTime.UtcNow;
        if (now < tournament.RegistrationStart || now > tournament.RegistrationEnd)
            throw new Exception("Реєстрація на цей турнір зараз закрита або ще не почалася");

        // 1.6 Перевіряємо ліміт команд (якщо він заданий)
        if (tournament.MaxTeams.HasValue)
        {
            var currentTeamsCount = await _context.Teams.CountAsync(t => t.TournamentId == dto.TournamentId);
            if (currentTeamsCount >= tournament.MaxTeams.Value)
                throw new Exception("На цей турнір вже зареєстровано максимальну кількість команд");
        }
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