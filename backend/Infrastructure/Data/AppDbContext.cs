using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<ShortLink> ShortLinks { get; set; }
    public DbSet<ClickEvent> ClickEvents { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTimeOffset.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ShortLink>()
            .HasOne(shortLink => shortLink.User)
            .WithMany()
            .HasForeignKey(shortLink => shortLink.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShortLink>()
            .HasIndex(shortLink => shortLink.ShortCode)
            .IsUnique();
        
        modelBuilder.Entity<ClickEvent>()
            .HasOne(clickEvent => clickEvent.ShortLink)
            .WithMany(shortLink => shortLink.ClickEvents)
            .HasForeignKey(clickEvent => clickEvent.ShortLinkId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClickEvent>()
            .HasIndex(clickEvent => clickEvent.CreatedAt);
    }
}
