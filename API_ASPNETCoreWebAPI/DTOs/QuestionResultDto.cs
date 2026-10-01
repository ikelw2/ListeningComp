namespace API_ASPNETCoreWebAPI.DTOs
{
    public class QuestionResultDto
    {
        public string QuestionId { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        // Numeric index (0-based) chosen by the user
        public int UserAnswer { get; set; }

        // The correct answer index (0-based)
        public int CorrectAnswer { get; set; }
    }
}
