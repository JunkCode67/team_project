using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentPlatform.Application.DTO.Evaluation;

namespace TournamentPlatform.Application.Interfaces;

public interface IEvaluationService
{
    // Метод, який ми бачили на вашому найпершому скріншоті: 
    // автоматичний розподіл робіт між членами журі
    Task AssignSubmissionsAsync(Guid roundId, int submissionsPerJury = 3);

    // Збереження нової оцінки від члена журі
    Task<EvaluationResponseDto> EvaluateAsync(EvaluateSubmissionDto dto);

    // Отримання турнірної таблиці для конкретного раунду 
    // (саме він використовує LeaderboardItemDto)
    Task<IEnumerable<LeaderboardItemDto>> GetLeaderboardAsync(Guid roundId);

    // Отримання всіх оцінок для конкретної заявки 
    // (щоб команда або адмін могли подивитися деталі оцінювання)
    Task<IEnumerable<EvaluationResponseDto>> GetEvaluationsBySubmissionIdAsync(Guid submissionId);
}