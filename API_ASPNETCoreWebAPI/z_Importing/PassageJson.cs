
///// <summary>
///// This file I heavily relied upon AI to help me 
///// import several JSON files for easily creating my 
///// data source, since I had several already-created
///// JSON files to use already...
///// </summary>








//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace API_ASPNETCoreWebAPI.Importing;

//public sealed class PassageJson
//{
//    [JsonPropertyName("sourcelink")]
//    // Accept string or array in JSON; parse at mapping time.
//    public JsonElement SourceLink { get; set; }

//    [JsonPropertyName("transcript")]
//    public List<string>? Transcript { get; set; }

//    [JsonPropertyName("translation")]
//    public List<string>? Translation { get; set; }

//    [JsonPropertyName("questions")]
//    public List<List<string>>? Questions { get; set; }
//}