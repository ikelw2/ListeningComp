namespace SharedDtoClassLibrary.DTOs
{
    // response DTO so user can listen to passage
    public record ListeningDto // instead of class, use record to optimize for init-only/immutable value-based data
    {
        public string SourceUrl { get; init; } = string.Empty;
        public string MediaUrl { get; init; } = string.Empty;

        public IReadOnlyList<string> Transcription { get; init; } = Array.Empty<string>(); // use IReadOnlyList and extra initializor to ensure immutability
        public IReadOnlyList<string> Translation { get; init; } = Array.Empty<string>();

        public IReadOnlyList<ListeningQuestionDto> Questions { get; init; } = Array.Empty<ListeningQuestionDto>();
    }

    public record ListeningQuestionDto
    {
        public string QuestionText { get; init; } = string.Empty;
        public IReadOnlyList<string> AnswerChoices { get; init; } = Array.Empty<string>();
    }
}
