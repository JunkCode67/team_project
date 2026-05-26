using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentPlatform.Application.DTO.Evaluation;
using TournamentPlatform.Application.Interfaces;

namespace TournamentPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EvaluationsController : ControllerBase
{
    private readonly IEvaluationService _evaluationService;

    public EvaluationsController(IEvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

    [HttpPost("evaluate")]
    public async Task<IActionResult> EvaluateSubmission([FromBody] EvaluateSubmissionDto dto)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid juryId))
            {
                return Unauthorized(new { message = "Не вдалося визначити користувача з токена. Перевірте авторизацію." });
            }

            dto.JuryId = juryId; 

            var result = await _evaluationService.EvaluateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("assign/{roundId:guid}")]
    public async Task<IActionResult> AssignSubmissions(Guid roundId, [FromQuery] int submissionsPerJury = 3)
    {
        try
        {
            await _evaluationService.AssignSubmissionsAsync(roundId, submissionsPerJury);
            return Ok(new { message = "Роботи успішно розподілені між членами журі." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("leaderboard/{roundId:guid}")]
    [AllowAnonymous] 
    public async Task<IActionResult> GetLeaderboard(Guid roundId)
    {
        var leaderboard = await _evaluationService.GetLeaderboardAsync(roundId);
        return Ok(leaderboard);
    }

    [HttpGet("submission/{submissionId:guid}")]
    public async Task<IActionResult> GetEvaluationsBySubmission(Guid submissionId)
    {
        var evaluations = await _evaluationService.GetEvaluationsBySubmissionIdAsync(submissionId);
        return Ok(evaluations);
    }
}