using TournamentPlatform.Application.DTO;
using TournamentPlatform.Application.DTO.Submission;

namespace TournamentPlatform.Application.Interfaces;

public interface ISubmissionService
{
    Task<SubmissionResponseDto> SubmitAsync(CreateSubmissionDto dto);
    Task<SubmissionResponseDto> UpdateAsync(Guid id, CreateSubmissionDto dto);
    Task<IEnumerable<SubmissionResponseDto>> GetByRoundIdAsync(Guid roundId);
    Task LockSubmissionsAsync(Guid roundId);
    Task<SubmissionResponseDto?> GetByIdAsync(Guid id);
    Task<SubmissionResponseDto?> GetByTeamAndRoundAsync(Guid teamId, Guid roundId);
}