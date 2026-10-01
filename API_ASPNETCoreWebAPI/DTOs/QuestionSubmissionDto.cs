namespace API_ASPNETCoreWebAPI.DTOs
{
    public class QuestionSubmissionDto
    {
        // Question Id uses string GUID in the domain model
        public string QuestionId { get; set; } = string.Empty;
        // Selected answer is an integer index (0-based) matching Question.AnswerChoices
        public int SelectedAnswer { get; set; }
    }
}
