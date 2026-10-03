namespace SharedDtoClassLibrary.DTOs
{
    // question properties required for validation & grading
    public record GradingDto
    {
        public IReadOnlyList<GradingAnswersDto> Questions { get; init; } = Array.Empty<GradingAnswersDto>();
    }

    public record GradingAnswersDto
    {
        public string QuestionId { get; init; } = Guid.NewGuid().ToString(); // use QuestionId explicity to prevent confounding of terms
        public int Position { get; init; }

        public int CorrectChoice { get; init; } // zero-based: A = 0, B = 1, C = 2, D = 3
        public int AnswerChoicesCount { get; init; } // to validate answer is within correct limits
    }
}
