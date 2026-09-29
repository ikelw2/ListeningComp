public class Passage
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // PRIMARY_KEY
    public string Transcription { get; set; } = string.Empty; // (this could run several paragraphs in length with newline characters etc)
    public string Translation { get; set; } = string.Empty; // (this is the english translation of the paragraphs; ordinarily I will use computer to translate to fill this)
    public string MediaUrl { get; set; } = string.Empty; // (location of mp3 file that plays the audio of the transcription)
    public string Language { get; set; } = string.Empty; // language of passage
    public string SourceUrl { get; set; } = string.Empty; // link to origin of content
    public List<Question> Questions { get; set; } = new();
}
