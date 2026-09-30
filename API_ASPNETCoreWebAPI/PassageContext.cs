using Microsoft.EntityFrameworkCore;

using API_ASPNETCoreWebAPI.Models;
namespace API_ASPNETCoreWebAPI;

public class PassageContext : DbContext
{
    public DbSet<Passage> Passages { get; set; } = null;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=passages.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Passage>().HasKey(c => c.Id);

        modelBuilder.Entity<Passage>().HasData(
            new Passage { 
                Language  = "RU",
                SourceUrl = "",
                MediaUrl  = "",
                Transcription = "",
                Translation   = "",
                Questions = new Question
            },
            new Passage { }            
         );
    }

}
    
    public string Id { get; set; } = Guid.NewGuid().ToString(); // PRIMARY_KEY
    public string QuestionText { get; set; } = string.Empty;
    public List<string> AnswerChoices { get; set; } = new(); // poss used for text of questions (contains multiple paragraphs with labeling)
    public int CorrectChoice { get; set; } = 99; // poss used to ID correct answer of questions (contains single-char/num label to match the correct option)
    public string PassageId { get; set; } = string.Empty; // FOREIGN_KEY (1 or more questions per passage)
