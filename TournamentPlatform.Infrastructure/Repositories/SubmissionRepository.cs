using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Infrastructure.Persistence;

namespace TournamentPlatform.Infrastructure.Repositories;

public class SubmissionRepository : GenericRepository<Submission>
{
    public SubmissionRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Submission>> GetByRoundIdAsync(Guid roundId)
        => await _context.Submissions
            .Include(s => s.Team)
            .Include(s => s.Evaluations)
            .Where(s => s.RoundId == roundId)
            .ToListAsync();

    public async Task<Submission?> GetByTeamAndRoundAsync(Guid teamId, Guid roundId)
        => await _context.Submissions
            .FirstOrDefaultAsync(s => s.TeamId == teamId && s.RoundId == roundId);
}