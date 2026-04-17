using System.Security.Cryptography.X509Certificates;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Links;

public class ClickEventRepository(AppDbContext context) : IClickEventRepository
{
    public async Task AddClickEventAsync(ClickEvent clickEvent)
    {
        context.ClickEvents.Add(clickEvent);
        await context.SaveChangesAsync();
    }

    public async Task<IList<ClicksByDateDto>> GetClickEvents30DaysByShortLinkIdAsync(int shortLinkId)
    {
        var since = DateTimeOffset.UtcNow.AddDays(-30);
        var clicks = await context.ClickEvents
            .Where(ce => 
                ce.ShortLinkId == shortLinkId
                && ce.CreatedAt >= since
            )
            .GroupBy(ce => new 
            {
                ce.CreatedAt.Year,
                ce.CreatedAt.Month,
                ce.CreatedAt.Day
            })
            .Select(g => new
            {
              g.Key.Year,
              g.Key.Month,
              g.Key.Day,
              Count = g.Count()  
            } )
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenBy(x => x.Day)
            .ToListAsync();
        return clicks.Select(x => new ClicksByDateDto
            {
                Date = new DateOnly
                    (
                        x.Year,
                        x.Month,
                        x.Day
                    ),
                Count = x.Count
            })
            .ToList();
    }

    public async Task<IList<ClickEvent>> GetClickEventsByShortLinkIdAsync(int shortLinkId)
    {
        return await context.ClickEvents.Where(ce => ce.ShortLinkId == shortLinkId).ToListAsync();
    }

    public async Task<int> GetClickEventsTodayByShortLinkIdAsync(int shortLinkId)
    {
        var now = DateTimeOffset.UtcNow;
        return await context.ClickEvents
            .CountAsync(ce => 
                ce.ShortLinkId == shortLinkId
                && ce.CreatedAt.Year == now.Year
                && ce.CreatedAt.Month == now.Month
                && ce.CreatedAt.Day == now.Day
            );
    }

    public async Task<int> GetClickEventsWeekByShortLinkIdAsync(int shortLinkId)
    {
        var since = DateTimeOffset.UtcNow.AddDays(-7);
        return await context.ClickEvents
            .CountAsync(ce => 
                ce.ShortLinkId == shortLinkId
                && ce.CreatedAt >= since
            );
    }

    public async Task<int> GetClicksTodayByUserAsync(string userId)
    {
        var now = DateTimeOffset.UtcNow;
        return await context.ClickEvents
            .CountAsync(ce => 
                ce.ShortLink.UserId == userId
                && ce.CreatedAt.Year == now.Year
                && ce.CreatedAt.Month == now.Month
                && ce.CreatedAt.Day == now.Day
            );
    }

    public async Task<IList<ReferrerDto>> GetTopReferrerByShortLinkIdAsync(int shortLinkId)
    {
        var clicks = await context.ClickEvents
            .Where(ce => ce.ShortLinkId == shortLinkId)
            .GroupBy(ce => ce.Referrer)
            .Select(g => new
            {
                g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync();
        return clicks.Select(x => new ReferrerDto
            {
                Referrer = x.Key,
                Count = x.Count
            })
            .ToList();
    }

    public async Task<int> GetTotalClickEventsByShortLinkIdAsync(int shortLinkId)
    {
        return await context.ClickEvents
            .CountAsync(ce => ce.ShortLinkId == shortLinkId);
    }

    public async Task<int> GetTotalClicksByUserAsync(string userId)
    {
        return await context.ClickEvents
            .CountAsync(ce => ce.ShortLink.UserId == userId);
    }
}