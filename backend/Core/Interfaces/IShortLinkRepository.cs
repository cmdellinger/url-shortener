using Core.Entities;

namespace Core.Interfaces;

public interface IShortLinkRepository
{
    Task AddShortLinkAsync(ShortLink shortLink);
    Task<IList<ShortLink>> GetShortLinksByUserAsync(string userId);
    Task<ShortLink?> GetShortLinkByIdAsync(int id);
    Task<ShortLink?> GetShortLinkByShortCodeAsync(string shortCode);
    Task<IList<ShortLink>> GetTopLinksByUserAsync(string userId, int count);
    Task UpdateShortLinkAsync(ShortLink shortLink);
    Task DeleteShortLinkAsync(ShortLink shortLink);
}
