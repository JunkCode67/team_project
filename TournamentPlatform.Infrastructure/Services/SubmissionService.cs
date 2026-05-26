using Microsoft.EntityFrameworkCore;
using TournamentPlatform.Application.DTO;
using TournamentPlatform.Application.DTO.Submission;
using TournamentPlatform.Application.Interfaces;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Domain.Enums;
using TournamentPlatform.Infrastructure.Persistence;
namespace TournamentPlatform.Infrastructure.Services;


public class SubmissionService : ISubmissionService
{
    private readonly UnitOfWork _uow;
    private readonly AppDbContext _context;

    public SubmissionService(UnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<SubmissionResponseDto> SubmitAsync(CreateSubmissionDto dto,Guid currentUserId)
    {
        var isMember = await _context.TeamMembers
            .AnyAsync(tm => tm.TeamId == dto.TeamId && tm.UserId == currentUserId);

        if (!isMember)
            throw new Exception("Ви не є учасником цієї команди і не можете відправляти за неї рішення!");

        var round = await _context.Rounds.FindAsync(dto.RoundId);
        if (round == null)
            throw new Exception("Раунд не знайдено");

        if (round.Status != RoundStatus.Active)
            throw new Exception("Раунд не активний");

        if (DateTime.UtcNow > round.Deadline)
            throw new Exception("Дедлайн минув");

        var existing = await _uow.Submissions
            .GetByTeamAndRoundAsync(dto.TeamId, dto.RoundId);

        if (existing != null)
            throw new Exception("Команда вже подала роботу");

        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            TeamId = dto.TeamId,
            RoundId = dto.RoundId,
            GitHubUrl = dto.GitHubUrl,
            VideoUrl = dto.VideoUrl,
            LiveDemoUrl = dto.LiveDemoUrl,
            Description = dto.Description,
            Status = SubmissionStatus.Submitted,
            SubmittedAt = DateTime.UtcNow
        };

        await _uow.Submissions.AddAsync(submission);
        await _uow.SaveChangesAsync();

        return await MapToDto(submission);
    }

    public async Task<SubmissionResponseDto> UpdateAsync(Guid id, CreateSubmissionDto dto)
    {
        var submission = await _uow.Submissions.GetByIdAsync(id);
        if (submission == null)
            throw new Exception("Сабміт не знайдено");

        if (submission.Status == SubmissionStatus.Locked)
            throw new Exception("Сабміт заблокований після дедлайну");

        var round = await _context.Rounds.FindAsync(submission.RoundId);
        if (round != null && DateTime.UtcNow > round.Deadline)
            throw new Exception("Дедлайн минув");

        submission.GitHubUrl = dto.GitHubUrl;
        submission.VideoUrl = dto.VideoUrl;
        submission.LiveDemoUrl = dto.LiveDemoUrl;
        submission.Description = dto.Description;

        await _uow.Submissions.UpdateAsync(submission);
        await _uow.SaveChangesAsync();

        return await MapToDto(submission);
    }

    public async Task<IEnumerable<SubmissionResponseDto>> GetByRoundIdAsync(Guid roundId)
    {
        var submissions = await _uow.Submissions.GetByRoundIdAsync(roundId);
        var result = new List<SubmissionResponseDto>();
        foreach (var s in submissions)
            result.Add(await MapToDto(s));
        return result;
    }

    public async Task LockSubmissionsAsync(Guid roundId)
    {
        var submissions = await _uow.Submissions.GetByRoundIdAsync(roundId);
        foreach (var s in submissions)
        {
            s.Status = SubmissionStatus.Locked;
            await _uow.Submissions.UpdateAsync(s);
        }
        await _uow.SaveChangesAsync();
    }

    public async Task<SubmissionResponseDto?> GetByIdAsync(Guid id)
    {
        var submission = await _uow.Submissions.GetByIdAsync(id);
        return submission == null ? null : await MapToDto(submission);
    }

    public async Task<SubmissionResponseDto?> GetByTeamAndRoundAsync(Guid teamId, Guid roundId)
    {
        var submission = await _uow.Submissions
            .GetByTeamAndRoundAsync(teamId, roundId);
        return submission == null ? null : await MapToDto(submission);
    }

    private async Task<SubmissionResponseDto> MapToDto(Submission s)
    {
        var team = await _context.Teams.FindAsync(s.TeamId);
        return new SubmissionResponseDto
        {
            Id = s.Id,
            TeamId = s.TeamId,
            TeamName = team?.Name ?? string.Empty,
            RoundId = s.RoundId,
            GitHubUrl = s.GitHubUrl,
            VideoUrl = s.VideoUrl,
            LiveDemoUrl = s.LiveDemoUrl,
            Description = s.Description,
            Status = s.Status,
            SubmittedAt = s.SubmittedAt
        };
    }
}