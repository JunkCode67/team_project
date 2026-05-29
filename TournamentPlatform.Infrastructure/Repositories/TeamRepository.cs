using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Infrastructure.Persistence;

namespace TournamentPlatform.Infrastructure.Repositories;

public class TeamRepository : GenericRepository<Team>
{
    public TeamRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Team>> GetByTournamentIdAsync(Guid tournamentId)
        => await _context.Teams
            .Include(t => t.Members)
            .Include(t => t.Captain)
            .Where(t => t.TournamentId == tournamentId)
            .ToListAsync();

    public async Task<bool> ExistsByEmailAsync(string email, Guid tournamentId)
        => await _context.TeamMembers
            .AnyAsync(m => m.Email == email &&
                           m.Team.TournamentId == tournamentId);
}