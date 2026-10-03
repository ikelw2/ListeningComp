namespace API_ASPNETCoreWebAPI.DTOs
{
    public class SubmissionDto
    {
        public string PassageId { get; set; } = string.Empty;
        public List<SubmissionAnswersDto> Answers { get; set; } = new();  // answers submitted by the user for grading
    }

    public class SubmissionAnswersDto 
    {
        public string QuestionId { get; set; } = string.Empty;
        public int? SelectedAnswer { get; set; }  // zero-based: A = 0, B = 1, C = 2, D = 3, null if no answer
    }
}
