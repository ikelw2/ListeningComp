namespace API_ASPNETCoreWebAPI.DTOs
{
    public class SubmissionFeedbackDto
    {

        public int TotalQuestions { get; set; }
        public int CorrectCount { get; set; }
        public List<QuestionResultDto> Results { get; set; } = new();
    }
}
