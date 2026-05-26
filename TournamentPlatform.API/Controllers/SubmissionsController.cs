using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentPlatform.Application.DTO.Submission;
using TournamentPlatform.Application.DTO;
using TournamentPlatform.Application.Interfaces;

namespace TournamentPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class SubmissionsController : ControllerBase
{
    private readonly ISubmissionService _submissionService;

    public SubmissionsController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionDto dto)
    {
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    
        if (!Guid.TryParse(userIdString, out Guid currentUserId))
        {
            return Unauthorized(new { message = "Не вдалося ідентифікувати користувача з токена." });
        }

        try
        {
            var result = await _submissionService.SubmitAsync(dto, currentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("round/{roundId:guid}")]
    [AllowAnonymous] 
    public async Task<IActionResult> GetByRound(Guid roundId)
    {
        var submissions = await _submissionService.GetByRoundIdAsync(roundId);
        return Ok(submissions);
    }
    
}