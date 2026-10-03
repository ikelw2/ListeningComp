namespace SharedDtoClassLibrary.DTOs
{
    // immutable response DTO for grading feedback
    public record SubmissionFeedbackDto
    {
        public int TotalQuestions { get; init; }
        public int CorrectCount { get; init; }
        public IReadOnlyList<SubmissionFeedbackAnswersDto> Results { get; init; } = Array.Empty<SubmissionFeedbackAnswersDto>();
    }

    public record SubmissionFeedbackAnswersDto
    {
        public string QuestionId { get; init; } = string.Empty;
        public bool IsCorrect { get; init; }

        public int? UserAnswer { get; init; }   // zero-based: A = 0, B = 1, C = 2, D = 3
        public int CorrectAnswer { get; init; } // zero-based: A = 0, B = 1, C = 2, D = 3
    }
}
