using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentPlatform.Application.DTO.Round;
using TournamentPlatform.Application.Interfaces;

namespace TournamentPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class RoundsController : ControllerBase
{
    private readonly IRoundService _roundService;

    public RoundsController(IRoundService roundService)
    {
        _roundService = roundService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRound([FromBody] CreateRoundDto dto)
    {
        try
        {
            var result = await _roundService.CreateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("tournament/{tournamentId:guid}")]
    [AllowAnonymous] 
    public async Task<IActionResult> GetByTournament(Guid tournamentId)
    {
        var rounds = await _roundService.GetByTournamentIdAsync(tournamentId);
        return Ok(rounds);
    }
}