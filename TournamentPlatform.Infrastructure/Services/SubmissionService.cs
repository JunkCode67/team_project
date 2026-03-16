using Microsoft.EntityFrameworkCore;
using TournamentPlatform.Application.DTO.Submission;
using TournamentPlatform.Application.DTOs;
using TournamentPlatform.Application.Interfaces;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Domain.Enums;
using TournamentPlatform.Infrastructure.Persistence;



public class SubmissionService : ISubmissionService
{
    private readonly UnitOfWork _uow;
    private readonly AppDbContext _context;

    public SubmissionService(UnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<SubmissionResponseDto> SubmitAsync(CreateSubmissionDto dto)
    {
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
            Submitted = DateTime.UtcNow
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

    public Task<SubmissionResponseDto?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<SubmissionResponseDto?> GetByTeamAndRoundAsync(Guid teamId, Guid roundId)
    {
        throw new NotImplementedException();
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
            SubmittedAt = s.Submitted
        };
    }
}