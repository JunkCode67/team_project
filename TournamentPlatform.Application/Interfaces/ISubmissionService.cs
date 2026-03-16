using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentPlatform.Application.DTO.Submission;
using TournamentPlatform.Application.DTOs;

namespace TournamentPlatform.Application.Interfaces;

public interface ISubmissionService
{
    // Створення нової заявки
    Task<SubmissionResponseDto> SubmitAsync(CreateSubmissionDto dto);

    // Отримання всіх заявок конкретного раунду (наприклад, для журі)
    Task<IEnumerable<SubmissionResponseDto>> GetByRoundIdAsync(Guid roundId);

    // Блокування заявок після завершення дедлайну
    Task LockSubmissionsAsync(Guid roundId);

    // Отримання конкретної заявки за її ID
    Task<SubmissionResponseDto?> GetByIdAsync(Guid id);

    // Отримання заявки конкретної команди у певному раунді (щоб перевірити, чи вони вже подавалися)
    Task<SubmissionResponseDto?> GetByTeamAndRoundAsync(Guid teamId, Guid roundId);
}