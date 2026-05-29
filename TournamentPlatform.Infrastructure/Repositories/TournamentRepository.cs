using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Infrastructure.Persistence;

namespace TournamentPlatform.Infrastructure.Repositories;

public class TournamentRepository : GenericRepository<Tournament>
{
    public TournamentRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Tournament>> GetAllWithTeamsAsync()
        => await _context.Tournaments
            .Include(t => t.Teams)
            .Include(t => t.Rounds)
            .ToListAsync();

    public async Task<Tournament?> GetByIdWithDetailsAsync(Guid id)
        => await _context.Tournaments
            .Include(t => t.Teams)
            .ThenInclude(t => t.Members)
            .Include(t => t.Rounds)
            .FirstOrDefaultAsync(t => t.Id == id);
}