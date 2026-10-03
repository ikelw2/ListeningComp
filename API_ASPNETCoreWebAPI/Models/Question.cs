namespace API_ASPNETCoreWebAPI.Models;

public class Question
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public int Position { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public List<string> AnswerChoices { get; set; } = new();

    // Zero-based: A = 0, B = 1, C = 2, D = 3.
    public int CorrectChoice { get; set; }

    public string PassageId { get; set; } = string.Empty;
}
