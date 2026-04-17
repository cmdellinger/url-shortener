using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces;

public interface IClickEventRepository
{
    Task AddClickEventAsync(ClickEvent clickEvent);
    Task<IList<ClickEvent>> GetClickEventsByShortLinkIdAsync(int shortLinkId);
    Task<int> GetTotalClicksByUserAsync(string userId);
    Task<int> GetClicksTodayByUserAsync(string userId);
    Task<int> GetTotalClickEventsByShortLinkIdAsync(int shortLinkId);
    Task<int> GetClickEventsTodayByShortLinkIdAsync(int shortLinkId);
    Task<int> GetClickEventsWeekByShortLinkIdAsync(int shortLinkId);
    Task<IList<ClicksByDateDto>> GetClickEvents30DaysByShortLinkIdAsync(int shortLinkId);
    Task<IList<ReferrerDto>> GetTopReferrerByShortLinkIdAsync(int shortLinkId);
}
