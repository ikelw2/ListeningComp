using API_ASPNETCoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace API_ASPNETCoreWebAPI.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Passage> Passages => Set<Passage>();
    public DbSet<Question> Questions => Set<Question>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Passage>()
            .HasIndex(p => p.ImportKey)
            .IsUnique();

        modelBuilder.Entity<Passage>()
            .HasMany(p => p.Questions)
            .WithOne()
            .HasForeignKey(q => q.PassageId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Question>()
            .HasIndex(q => new { q.PassageId, q.Position })
            .IsUnique();
    }
}