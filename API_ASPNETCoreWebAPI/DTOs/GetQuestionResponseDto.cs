namespace API_ASPNETCoreWebAPI.DTOs;

public class GetQuestionResponseDto
{
    public string QuestionText { get; set; } = string.Empty;
    public List<string> AnswerChoices { get; set; } = new();

}