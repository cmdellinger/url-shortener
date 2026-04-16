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

    public async Task<IList<ClickEvent>> GetClickEventsByShortLinkIdAsync(int shortLinkId)
    {
        return await context.ClickEvents.Where(ce => ce.ShortLinkId == shortLinkId).ToListAsync();
    }
}