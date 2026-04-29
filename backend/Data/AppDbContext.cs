using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Participant> Participants => Set<Participant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(255);
            e.Property(x => x.StartDate).IsRequired();
            e.Property(x => x.EndDate).IsRequired();
            e.Property(x => x.MaxParticipants).IsRequired();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<Participant>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            e.Property(x => x.PersonalIdCode).IsRequired().HasMaxLength(20);
            e.Property(x => x.RegisteredAt).HasDefaultValueSql("NOW()");
            e.HasOne(x => x.Event)
             .WithMany(x => x.Participants)
             .HasForeignKey(x => x.EventId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => new { x.EventId, x.PersonalIdCode }).IsUnique();
        });
    }
}