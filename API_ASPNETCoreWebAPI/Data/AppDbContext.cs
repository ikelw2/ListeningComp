using API_ASPNETCoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace API_ASPNETCoreWebAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Passage> Passages => Set<Passage>();
    public DbSet<Question> Questions => Set<Question>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) // code-first implementation allows this code to create the database
    {
        base.OnModelCreating(modelBuilder); // best practice: run base class OnModelCreating first

        modelBuilder.Entity<Passage>()
            .HasIndex(p => p.ImportKey) // creates unique constraint on ImportKey field, enforces uniqueness when importing json files
            .IsUnique();

        modelBuilder.Entity<Passage>() // creates one-Passage to many-Questions relationship
            .HasMany(p => p.Questions)
            .WithOne()
            .HasForeignKey(q => q.PassageId) // with foreign key associated with PassageId
            .OnDelete(DeleteBehavior.Cascade); // and cascade on delete behavior

        modelBuilder.Entity<Question>() // creates composite index consisting of PassageId and Position
            .HasIndex(q => new { q.PassageId, q.Position })
            .IsUnique();
    }
}