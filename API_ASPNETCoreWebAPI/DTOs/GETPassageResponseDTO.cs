using API_ASPNETCoreWebAPI.Models;

namespace API_ASPNETCoreWebAPI.DTOs;

public class GetPassageResponseDto
{
    public string SourceUrl { get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;

    public List<string> Transcription { get; set; } = new();
    public List<string> Translation { get; set; } = new();

    public List<GetQuestionResponseDto> Questions { get; set; } = new();
}

