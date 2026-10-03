namespace SharedDtoClassLibrary.DTOs
{
    // Mutable request DTO for incoming submissions (keeps compatibility with Blazor form binding)
    public class SubmissionDto
    {
        // Passage Id uses string GUID in the domain model
        public string PassageId { get; set; } = string.Empty;

        // Answers submitted by the user for grading
        public List<SubmissionAnswerDto> Answers { get; set; } = new();
    }

    public class SubmissionAnswerDto
    {
        public string QuestionId { get; set; } = string.Empty;
        public int? SelectedAnswer { get; set; }
    }
}
