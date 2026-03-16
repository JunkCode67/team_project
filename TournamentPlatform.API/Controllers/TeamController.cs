using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentPlatform.Application.DTO.Team;
using TournamentPlatform.Application.Interfaces;

namespace TournamentPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Обов'язково вимагаємо токен, щоб знати, хто створює команду
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterTeam([FromBody] CreateTeamDto dto)
    {
        try
        {
            // Витягуємо ID користувача з токена (він стане капітаном)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid captainId))
            {
                return Unauthorized(new { message = "Не вдалося визначити користувача. Перевірте токен авторизації." });
            }

            // Передаємо DTO та ID капітана в сервіс
            var result = await _teamService.RegisterTeamAsync(dto, captainId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Якщо команда з такою назвою вже є, або турнір не знайдено
            return BadRequest(new { message = ex.Message });
        }
    }
}