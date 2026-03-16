using Microsoft.EntityFrameworkCore;
using TournamentPlatform.Application.DTO.Evaluation;
using TournamentPlatform.Application.DTO.Evaluation;
using TournamentPlatform.Application.Interfaces;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Domain.Enums;
using TournamentPlatform.Infrastructure.Persistence;

namespace TournamentPlatform.Infrastructure.Services;

public class EvaluationService : IEvaluationService
{
    private readonly UnitOfWork _uow;
    private readonly AppDbContext _context;

    public EvaluationService(UnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task AssignSubmissionsAsync(Guid roundId, int submissionsPerJury = 3)
    {
        var submissions = await _uow.Submissions.GetByRoundIdAsync(roundId);
        var submissionList = submissions.ToList();

        var juryMembers = await _context.Users
            .Where(u => u.Role == UserRole.Jury)
            .ToListAsync();

        if (!juryMembers.Any())
            throw new Exception("Немає членів журі");

        var random = new Random();

        foreach (var jury in juryMembers)
        {
            var shuffled = submissionList
                .OrderBy(_ => random.Next())
                .Take(submissionsPerJury)
                .ToList();

            foreach (var submission in shuffled)
            {
                var alreadyAssigned = await _uow.Evaluations
                    .AlreadyEvaluatedAsync(jury.Id, submission.Id);

                if (!alreadyAssigned)
                {
                    var evaluation = new Evaluation
                    {
                        Id = Guid.NewGuid(),
                        SubmissionId = submission.Id,
                        JuryId = jury.Id,
                        EvaluatedAt = DateTime.UtcNow
                    };
                    await _uow.Evaluations.AddAsync(evaluation);
                }
            }
        }

        await _uow.SaveChangesAsync();
    }

    public async Task<EvaluationResponseDto> EvaluateAsync(EvaluateSubmissionDto dto)
    {
        var alreadyEvaluated = await _uow.Evaluations
            .AlreadyEvaluatedAsync(dto.JuryId, dto.SubmissionId);

        if (alreadyEvaluated)
            throw new Exception("Ця робота вже оцінена цим журі");

        var evaluation = new Evaluation
        {
            Id = Guid.NewGuid(),
            SubmissionId = dto.SubmissionId,
            JuryId = dto.JuryId,
            ScoreBackend = dto.ScoreBackend,
            ScoreDatabase = dto.ScoreDatabase,
            ScoreFrontend = dto.ScoreFrontend,
            ScoreFunctionality = dto.ScoreFunctionality,
            ScoreUsability = dto.ScoreUsability,
            Comment = dto.Comment,
            EvaluatedAt = DateTime.UtcNow
        };

        await _uow.Evaluations.AddAsync(evaluation);
        await _uow.SaveChangesAsync();

        return await MapToDto(evaluation);
    }

    public async Task<IEnumerable<EvaluationResponseDto>> GetBySubmissionIdAsync(Guid submissionId)
    {
        var evaluations = await _uow.Evaluations.GetBySubmissionIdAsync(submissionId);
        var result = new List<EvaluationResponseDto>();
        foreach (var e in evaluations)
            result.Add(await MapToDto(e));
        return result;
    }

    public async Task<IEnumerable<LeaderboardItemDto>> GetLeaderboardAsync(Guid roundId)
    {
        var submissions = await _uow.Submissions.GetByRoundIdAsync(roundId);

        var leaderboard = new List<LeaderboardItemDto>();

        foreach (var submission in submissions)
        {
            var evaluations = await _uow.Evaluations
                .GetBySubmissionIdAsync(submission.Id);

            var evalList = evaluations.ToList();
            if (!evalList.Any()) continue;

            var team = await _context.Teams.FindAsync(submission.TeamId);

            leaderboard.Add(new LeaderboardItemDto
            {
                TeamId = submission.TeamId,
                TeamName = team?.Name ?? string.Empty,
                AverageScore = evalList.Average(e =>
                    e.ScoreBackend + e.ScoreDatabase +
                    e.ScoreFrontend + e.ScoreFunctionality +
                    e.ScoreUsability),
                TotalEvaluations = evalList.Count,
                ScoreBackend = evalList.Average(e => e.ScoreBackend),
                ScoreDatabase = evalList.Average(e => e.ScoreDatabase),
                ScoreFrontend = evalList.Average(e => e.ScoreFrontend),
                ScoreFunctionality = evalList.Average(e => e.ScoreFunctionality),
                ScoreUsability = evalList.Average(e => e.ScoreUsability)
            });
        }

        return leaderboard.OrderByDescending(l => l.AverageScore);
    }

    public Task<IEnumerable<EvaluationResponseDto>> GetEvaluationsBySubmissionIdAsync(Guid submissionId)
    {
        throw new NotImplementedException();
    }

    private async Task<EvaluationResponseDto> MapToDto(Evaluation e)
    {
        var submission = await _context.Submissions
            .Include(s => s.Team)
            .FirstOrDefaultAsync(s => s.Id == e.SubmissionId);

        var jury = await _context.Users.FindAsync(e.JuryId);

        return new EvaluationResponseDto
        {
            Id = e.Id,
            SubmissionId = e.SubmissionId,
            TeamName = submission?.Team?.Name ?? string.Empty,
            JuryName = jury?.Name ?? string.Empty,
            ScoreBackend = e.ScoreBackend,
            ScoreDatabase = e.ScoreDatabase,
            ScoreFrontend = e.ScoreFrontend,
            ScoreFunctionality = e.ScoreFunctionality,
            ScoreUsability = e.ScoreUsability,
            TotalScore = e.ScoreBackend + e.ScoreDatabase +
                        e.ScoreFrontend + e.ScoreFunctionality +
                        e.ScoreUsability,
            Comment = e.Comment,
            EvaluatedAt = e.EvaluatedAt
        };
    }
}