using System.ComponentModel.DataAnnotations;

namespace TournamentPlatform.Application.DTO.Evaluation;

public class EvaluateSubmissionDto
{
    public Guid SubmissionId { get; set; }
    
    // Це поле ми заповнюємо в контролері з токена
    public Guid JuryId { get; set; } 

    [Range(0, 100, ErrorMessage = "Бал за Backend має бути від 0 до 100.")]
    public int ScoreBackend { get; set; }

    [Range(0, 100, ErrorMessage = "Бал за Database має бути від 0 до 100.")]
    public int ScoreDatabase { get; set; }

    [Range(0, 100, ErrorMessage = "Бал за Frontend має бути від 0 до 100.")]
    public int ScoreFrontend { get; set; }

    [Range(0, 100, ErrorMessage = "Бал за Функціональність має бути від 0 до 100.")]
    public int ScoreFunctionality { get; set; }

    [Range(0, 100, ErrorMessage = "Бал за Юзабіліті має бути від 0 до 100.")]
    public int ScoreUsability { get; set; }

    public string? Comment { get; set; }
}