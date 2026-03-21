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

    public async Task AssignSubmissionsAsync(Guid roundId, int submissionsPerJury)
    {
        // 1. Беремо всі роботи цього раунду, які ще не мають оцінок
        var submissions = await _context.Submissions
            .Where(s => s.RoundId == roundId)
            .ToListAsync();

        // 2. Беремо всіх активних суддів
        var juries = await _context.Users
            .Where(u => u.Role == UserRole.Jury) // Перевір, як у тебе називається роль в Enum
            .ToListAsync();

        if (!juries.Any()) throw new Exception("У системі немає зареєстрованих суддів!");
        if (!submissions.Any()) throw new Exception("Для цього раунду ще не подано жодної роботи.");

        var random = new Random();
        var shuffledSubmissions = submissions.OrderBy(x => random.Next()).ToList();

        int juryIndex = 0;

        // 3. Розподіляємо (спрощена логіка: кожен суддя отримує наступну роботу по черзі)
        foreach (var submission in shuffledSubmissions)
        {
            // Перевіряємо, чи ця робота вже не призначена цьому судді (щоб не було дублів)
            var alreadyAssigned = await _context.Evaluations
                .AnyAsync(e => e.SubmissionId == submission.Id && e.JuryId == juries[juryIndex].Id);

            if (!alreadyAssigned)
            {
                var assignment = new Evaluation
                {
                    Id = Guid.NewGuid(),
                    SubmissionId = submission.Id,
                    JuryId = juries[juryIndex].Id,
                    // Бали залишаємо порожніми (або 0), поки суддя не перевірить
                    ScoreBackend = 0,
                    ScoreDatabase = 0,
                    ScoreFrontend = 0,
                    ScoreFunctionality = 0,
                    ScoreUsability = 0,
                    EvaluatedAt = DateTime.UtcNow
                };
                _context.Evaluations.Add(assignment);
            }

            // Переходимо до наступного судді (циклічно)
            juryIndex = (juryIndex + 1) % juries.Count;
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task<EvaluationResponseDto> EvaluateAsync(EvaluateSubmissionDto dto)
    {
        // 1. Шукаємо призначену роботу (пустишку).
        // Тобі потрібно буде додати метод GetByJuryAndSubmissionAsync у твій репозиторій,
        // який поверне сутність Evaluation за цими двома ID.
        var evaluation = await _uow.Evaluations.GetByJuryAndSubmissionAsync(dto.JuryId, dto.SubmissionId);

        // Якщо Адмін не призначав цю роботу цьому судді
        if (evaluation == null)
            throw new Exception("Помилка: Ця робота вам не призначена!");

        // 2. Перевіряємо, чи суддя вже виставив реальні бали раніше.
        // Наприклад, перевіряємо, чи коментар вже заповнений, або чи є хоч один бал.
        if (!string.IsNullOrEmpty(evaluation.Comment)) 
            throw new Exception("Ця робота вже оцінена цим журі!");

        // 3. Заповнюємо "пустишку" реальними оцінками
        evaluation.ScoreBackend = dto.ScoreBackend;
        evaluation.ScoreDatabase = dto.ScoreDatabase;
        evaluation.ScoreFrontend = dto.ScoreFrontend;
        evaluation.ScoreFunctionality = dto.ScoreFunctionality;
        evaluation.ScoreUsability = dto.ScoreUsability;
        evaluation.Comment = dto.Comment;
        evaluation.EvaluatedAt = DateTime.UtcNow;

        // 4. Просто зберігаємо зміни! EF Core сам зрозуміє, що треба зробити UPDATE
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

   public async Task<IEnumerable<LeaderBoardItemDto>> GetLeaderboardAsync(Guid roundId)
{
    // 1. Отримуємо всі сабміти для цього раунду разом із командою та оцінками
    var submissions = await _context.Submissions// або _context.Submissions
        .Include(s => s.Team)
        .Include(s => s.Evaluations)
        .Where(s => s.RoundId == roundId)
        .ToListAsync();

    var leaderboard = new List<LeaderBoardItemDto>();

    // 2. Рахуємо бали для кожної команди
    foreach (var submission in submissions)
    {
        // ВІДФІЛЬТРОВУЄМО "ПУСТИШКИ": беремо тільки ті оцінки, де суддя вже залишив коментар
        var completedEvaluations = submission.Evaluations
            .Where(e => !string.IsNullOrEmpty(e.Comment))
            .ToList();

        int evaluationsCount = completedEvaluations.Count;

        // Змінні для збереження фінальних балів (за замовчуванням 0)
        int avgBackend = 0;
        int avgDatabase = 0;
        int avgFrontend = 0;
        int avgFunctionality = 0;
        int avgUsability = 0;
        int totalAvgScore = 0;

        // Якщо роботу оцінив хоча б один суддя, рахуємо середнє
        if (evaluationsCount > 0)
        {
            avgBackend = (int)Math.Round(completedEvaluations.Average(e => e.ScoreBackend));
            avgDatabase = (int)Math.Round(completedEvaluations.Average(e => e.ScoreDatabase));
            avgFrontend = (int)Math.Round(completedEvaluations.Average(e => e.ScoreFrontend));
            avgFunctionality = (int)Math.Round(completedEvaluations.Average(e => e.ScoreFunctionality));
            avgUsability = (int)Math.Round(completedEvaluations.Average(e => e.ScoreUsability));

            // Загальний бал - це сума середніх балів за всіма критеріями
            totalAvgScore = avgBackend + avgDatabase + avgFrontend + avgFunctionality + avgUsability;
        }

        // 3. Заповнюємо DTO всіма розрахованими полями
        leaderboard.Add(new LeaderBoardItemDto
        {
            TeamId = submission.TeamId,
            TeamName = submission.Team?.Name ?? "Невідома команда",
            SubmissionId = submission.Id,
            AverageScore = totalAvgScore,
            TotalEvaluations = evaluationsCount,
            ScoreBackend = avgBackend,
            ScoreDatabase = avgDatabase,
            ScoreFrontend = avgFrontend,
            ScoreFunctionality = avgFunctionality,
            ScoreUsability = avgUsability
        });
    }

    // 4. Сортуємо від переможців (найбільший бал) до тих, хто набрав менше
    return leaderboard.OrderByDescending(x => x.AverageScore).ToList();
}

    public Task<Evaluation?> GetByJuryAndSubmissionAsync(Guid juryId, Guid submissionId)
    {
        throw new NotImplementedException();
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