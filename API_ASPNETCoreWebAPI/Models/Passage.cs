public class Passage
{
    public int PassageId { get; set; } // PRIMARY_KEY
    public string Transcription { get; set; } // (this could run several paragraphs in length with newline characters etc)
    public string Translation { get; set; } // (this is the english translation of the paragraphs; ordinarily I will use computer to translate to fill this)
    public string MediaUrl { get; set; } // (location of mp3 file that plays the audio of the transcription)
    public string Language { get; set; } // language of passage
    public DateTime SourceDate { get; set; }
    public string SourceUrl { get; set; } // link to origin of content
    public List<Question> Questions { get; set; }
}
