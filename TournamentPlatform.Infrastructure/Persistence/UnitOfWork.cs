using TournamentPlatform.Domain.Interfaces;
using TournamentPlatform.Infrastructure.Repositories;

namespace TournamentPlatform.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public TournamentRepository Tournaments { get; }
    public TeamRepository Teams { get; }
    public SubmissionRepository Submissions { get; }
    public EvaluationRepository Evaluations { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Tournaments = new TournamentRepository(context);
        Teams = new TeamRepository(context);
        Submissions = new SubmissionRepository(context);
        Evaluations = new EvaluationRepository(context);
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
}