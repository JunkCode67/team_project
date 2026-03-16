using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentPlatform.Application.DTO.Evaluation;
using TournamentPlatform.Application.Interfaces;

namespace TournamentPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // За замовчуванням вимагаємо токен для більшості дій з оцінками
public class EvaluationsController : ControllerBase
{
    private readonly IEvaluationService _evaluationService;

    public EvaluationsController(IEvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

    // 1. Збереження оцінки від члена журі
    [HttpPost("evaluate")]
    // [Authorize(Roles = "Jury")] // Розкоментуй це пізніше, якщо додаси перевірку ролей
    public async Task<IActionResult> EvaluateSubmission([FromBody] EvaluateSubmissionDto dto)
    {
        try
        {
            // Безпечно дістаємо ID користувача (журі) з JWT токена
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid juryId))
            {
                return Unauthorized(new { message = "Не вдалося визначити користувача з токена. Перевірте авторизацію." });
            }

            dto.JuryId = juryId; // Записуємо витягнутий ID у наш об'єкт

            var result = await _evaluationService.EvaluateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Якщо щось пішло не так (наприклад, роботу вже оцінено), повертаємо 400
            return BadRequest(new { message = ex.Message });
        }
    }

    // 2. Автоматичний розподіл робіт (зазвичай це робить адмін турніру)
    [HttpPost("assign/{roundId:guid}")]
    // [Authorize(Roles = "Admin")]
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

    // 3. Отримання турнірної таблиці (може бути доступно всім)
    [HttpGet("leaderboard/{roundId:guid}")]
    [AllowAnonymous] // Дозволяємо дивитися таблицю без авторизації
    public async Task<IActionResult> GetLeaderboard(Guid roundId)
    {
        var leaderboard = await _evaluationService.GetLeaderboardAsync(roundId);
        return Ok(leaderboard);
    }

    // 4. Перегляд усіх оцінок конкретної роботи (деталізація)
    [HttpGet("submission/{submissionId:guid}")]
    public async Task<IActionResult> GetEvaluationsBySubmission(Guid submissionId)
    {
        var evaluations = await _evaluationService.GetEvaluationsBySubmissionIdAsync(submissionId);
        return Ok(evaluations);
    }
}