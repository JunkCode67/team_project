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
    [HttpGet("tournament/{tournamentId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTeamsByTournament(Guid tournamentId)
    {
        var teams = await _teamService.GetTeamsByTournamentAsync(tournamentId);
        return Ok(teams);
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
    [HttpPost("members")]
    [Authorize] // Запит може робити тільки авторизований юзер
    public async Task<IActionResult> AddMember([FromBody] AddTeamMemberDto dto)
    {
        // Витягуємо ID юзера з його JWT токена
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out Guid currentUserId))
        {
            return Unauthorized("Не вдалося визначити користувача");
        }

        try
        {
            await _teamService.AddMemberAsync(dto, currentUserId);
            return Ok(new { message = "Учасника успішно додано до команди!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("{teamId}/members")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMembers(Guid teamId)
    {
        try
        {
            var members = await _teamService.GetTeamMembersAsync(teamId);
            return Ok(members);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}