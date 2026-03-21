using Microsoft.EntityFrameworkCore;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Infrastructure.Persistence;

namespace TournamentPlatform.Infrastructure.Repositories;

public class EvaluationRepository : GenericRepository<Evaluation>
{
    public EvaluationRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Evaluation>> GetBySubmissionIdAsync(Guid submissionId)
        => await _context.Evaluations
            .Include(e => e.Jury)
            .Where(e => e.SubmissionId == submissionId)
            .ToListAsync();
    public async Task<Evaluation?> GetByJuryAndSubmissionAsync(Guid juryId, Guid submissionId)
    {
        return await _context.Evaluations
            .FirstOrDefaultAsync(e => e.JuryId == juryId && e.SubmissionId == submissionId);
    }
    public async Task<bool> AlreadyEvaluatedAsync(Guid juryId, Guid submissionId)
        => await _context.Evaluations
            .AnyAsync(e => e.JuryId == juryId && e.SubmissionId == submissionId);
}