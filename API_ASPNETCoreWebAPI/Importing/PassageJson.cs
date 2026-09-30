
using System.Text.Json.Serialization;

namespace API_ASPNETCoreWebAPI.Importing;

public sealed class PassageJson
{
    [JsonPropertyName("sourcelink")]
    public string? SourceLink { get; set; }

    [JsonPropertyName("transcript")]
    public List<string>? Transcript { get; set; }

    [JsonPropertyName("translation")]
    public List<string>? Translation { get; set; }

    [JsonPropertyName("questions")]
    public List<List<string>>? Questions { get; set; }
}