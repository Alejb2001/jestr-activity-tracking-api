using ActivityTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.AssignedUserId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).HasConversion<string>();
        });
    }
}
