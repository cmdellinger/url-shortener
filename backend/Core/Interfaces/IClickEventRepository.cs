using Core.Entities;

namespace Core.Interfaces;

public interface IClickEventRepository
{
    Task AddClickEventAsync(ClickEvent clickEvent);
    Task<IList<ClickEvent>> GetClickEventsByShortLinkIdAsync(int shortLinkId);
}
