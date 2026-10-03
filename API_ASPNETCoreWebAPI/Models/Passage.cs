namespace API_ASPNETCoreWebAPI.Models;

public class Passage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Relative source filename, used to prevent duplicate imports.
    // Nullable so passages created outside the importer need no import key.
    public string? ImportKey { get; set; }

    public string Language { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;

    public List<string> Transcription { get; set; } = new();
    public List<string> Translation { get; set; } = new();

    public List<Question> Questions { get; set; } = new();
}