using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentPlatform.Application.DTO.Tournament;
using TournamentPlatform.Application.Interfaces;
using TournamentPlatform.Domain.Enums;

namespace TournamentPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _tournamentService;

    public TournamentsController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tournaments = await _tournamentService.GetAllAsync();
        return Ok(tournaments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);
        if (tournament == null)
            return NotFound(new { message = "Турнір не знайдено" });
        return Ok(tournament);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateTournamentDto dto)
    {
        try
        {
            var result = await _tournamentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] TournamentStatus status)
    {
        try
        {
            var result = await _tournamentService.UpdateStatusAsync(id, status);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}