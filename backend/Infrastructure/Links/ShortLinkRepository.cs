using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Links;

public class ShortLinkRepository(AppDbContext context) : IShortLinkRepository
{
    public async Task AddShortLinkAsync(ShortLink shortLink)
    {
        context.ShortLinks.Add(shortLink);
        await context.SaveChangesAsync();
    }

    public async Task DeleteShortLinkAsync(ShortLink shortLink)
    {
        context.ShortLinks.Remove(shortLink);
        await context.SaveChangesAsync();
    }

    public async Task<ShortLink?> GetShortLinkByIdAsync(int id)
    {
        return await context.ShortLinks.FindAsync(id);
    }

    public async Task<ShortLink?> GetShortLinkByShortCodeAsync(string shortCode)
    {
        return await context.ShortLinks
            .Where(shortLink => shortLink.ShortCode == shortCode)
            .FirstOrDefaultAsync();
    }

    public async Task<IList<ShortLink>> GetShortLinksByUserAsync(string userId)
    {
        return await context.ShortLinks
            .Where(shortLink => shortLink.UserId == userId)
            .Include(shortLink => shortLink.ClickEvents)
            .OrderByDescending(shortLink => shortLink.CreatedAt)
            .ToListAsync();
    }

    public async Task<IList<ShortLink>> GetTopLinksByUserAsync(string userId, int count)
    {
        return await context.ShortLinks
            .Where(link => link.UserId == userId)
            .Include(link => link.ClickEvents)
            .OrderByDescending(link => link.ClickEvents.Count)
            .Take(count)
            .ToListAsync();
    }

    public async Task UpdateShortLinkAsync(ShortLink shortLink)
    {
        context.Entry(shortLink).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }
}
