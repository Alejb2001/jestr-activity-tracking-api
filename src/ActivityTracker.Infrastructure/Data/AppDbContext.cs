using ActivityTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasOne(e => e.Company)
                  .WithMany(c => c.AppUsers)
                  .HasForeignKey(e => e.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.AssignedUserId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Priority).HasConversion<string>();
            entity.HasOne(e => e.Company)
                  .WithMany(c => c.Activities)
                  .HasForeignKey(e => e.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasOne(e => e.Company)
                  .WithMany(c => c.Users)
                  .HasForeignKey(e => e.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        });
    }
}
