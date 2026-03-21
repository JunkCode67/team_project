using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentPlatform.Application.DTO.Submission;
using TournamentPlatform.Application.DTO;
using TournamentPlatform.Application.Interfaces;

namespace TournamentPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Вимагаємо токен для доступу до методів
public class SubmissionsController : ControllerBase
{
    private readonly ISubmissionService _submissionService;

    // Впровадження залежності (DI) нашого сервісу
    public SubmissionsController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionDto dto)
    {
        // 1. Читаємо токен і витягуємо ID юзера
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    
        // 2. Якщо токена немає, або він кривий - кажемо "До побачення"
        if (!Guid.TryParse(userIdString, out Guid currentUserId))
        {
            return Unauthorized(new { message = "Не вдалося ідентифікувати користувача з токена." });
        }

        try
        {
            // 3. Тепер передаємо ОБИДВА параметри: і дані (dto), і ID юзера (currentUserId)
            var result = await _submissionService.SubmitAsync(dto, currentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("round/{roundId:guid}")]
    [AllowAnonymous] // Наприклад, дозволяємо дивитися роботи без авторизації
    public async Task<IActionResult> GetByRound(Guid roundId)
    {
        var submissions = await _submissionService.GetByRoundIdAsync(roundId);
        return Ok(submissions);
    }
    
    // Сюди ж можна додати GetById та інші методи з інтерфейсу
}