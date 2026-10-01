namespace API_ASPNETCoreWebAPI.DTOs
{
    public class PassageSubmissionDto
    {
        // Passage Id uses string GUID in the domain model
        public string PassageId { get; set; } = string.Empty;
        public List<QuestionSubmissionDto> Answers { get; set; } = new();
    }
}
