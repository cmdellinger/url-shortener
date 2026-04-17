using System;
using Core.Dtos;
using Core.Interfaces;

namespace Infrastructure.Links;

public class DashboardService(IShortLinkRepository linkRepo,
                            IClickEventRepository clickEventRepo) : IDashboardService
{
    public async Task<DashboardDto> GetDashboardAsync(string userId)
    {
        var links = await linkRepo.GetShortLinksByUserAsync(userId);
        var topLinks = await linkRepo.GetTopLinksByUserAsync(userId, 5);
        return new DashboardDto
        {
            TotalLinks = links.Count,
            TotalClicks = await clickEventRepo.GetTotalClicksByUserAsync(userId),
            ClicksToday = await clickEventRepo.GetClicksTodayByUserAsync(userId),
            TopLinks = topLinks.Select(LinkDto.FromEntity).ToList()
        };
    }
}