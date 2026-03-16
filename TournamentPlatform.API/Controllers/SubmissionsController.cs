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
    public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionDto dto)
    {
        try
        {
            var result = await _submissionService.SubmitAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Якщо раунд не знайдено або дедлайн минув, повертаємо помилку 400 (Bad Request)
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