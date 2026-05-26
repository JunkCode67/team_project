using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentPlatform.Application.DTO.Evaluation;
using TournamentPlatform.Domain.Entities;

namespace TournamentPlatform.Application.Interfaces;

public interface IEvaluationService
{
    Task AssignSubmissionsAsync(Guid roundId, int submissionsPerJury = 3);

    Task<EvaluationResponseDto> EvaluateAsync(EvaluateSubmissionDto dto);


    Task<IEnumerable<LeaderBoardItemDto>> GetLeaderboardAsync(Guid roundId);
    
    
    Task<Evaluation?> GetByJuryAndSubmissionAsync(Guid juryId, Guid submissionId);

    Task<IEnumerable<EvaluationResponseDto>> GetEvaluationsBySubmissionIdAsync(Guid submissionId);
}
