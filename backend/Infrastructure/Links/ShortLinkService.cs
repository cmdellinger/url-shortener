using Core.Dtos;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Links;

public class ShortLinkService(IShortLinkRepository linkRepo,
                              IShortCodeGenerator codeGenerator,
                              IClickEventRepository clickEventRepo) : IShortLinkService
{
    public async Task<LinkDto> CreateShortLinkAsync(CreateLinkDto createLinkDto, string userId)
    {
        var newShortLink = new ShortLink
        {
            ShortCode = await codeGenerator.GenerateShortCode(),
            OriginalUrl = createLinkDto.OriginalUrl,
            Title = createLinkDto.Title,
            UserId = userId,
            IsActive = true
        };
        await linkRepo.AddShortLinkAsync(newShortLink);
        return LinkDto.FromEntity(newShortLink);
    }

    public async Task<bool> DeleteShortLinkAsync(int id, string userId)
    {
        var link = await linkRepo.GetShortLinkByIdAsync(id);
        if (link == null || link.UserId != userId) return false;

        await linkRepo.DeleteShortLinkAsync(link);
        return true;
    }

    public async Task<IList<LinkDto>> GetListOfShortLinksAsync(string userId)
    {
        var shortLinks = await linkRepo.GetShortLinksByUserAsync(userId);
        return shortLinks.Select(link => LinkDto.FromEntity(link)).ToArray();
    }

    public async Task<LinkDto?> GetShortLinkAsync(int id, string userId)
    {
        var link = await linkRepo.GetShortLinkByIdAsync(id);
        if (link == null || link.UserId != userId) return null;
        return LinkDto.FromEntity(link);
    }

    public async Task<string?> ResolveAndTrackAsync(string shortCode, string? ipAddress, string? userAgent, string? referrer)
    {
        var link = await linkRepo.GetShortLinkByShortCodeAsync(shortCode);
        if (link == null
            || !link.IsActive
            || (link.ExpiresAt.HasValue && DateTimeOffset.UtcNow > link.ExpiresAt))
        {
            return null;        
        }

        await clickEventRepo.AddClickEventAsync(
            new ClickEvent
            {
                ShortLinkId = link.Id,
                Referrer = referrer,
                UserAgent = userAgent,
                IpAddress = ipAddress
            }
        );
        return link.OriginalUrl;
    }

    public async Task<LinkDto?> UpdateLinkAsync(int id, UpdateLinkDto updateLinkDto, string userId)
    {
        var link = await linkRepo.GetShortLinkByIdAsync(id);
        if (link == null || link.UserId != userId) return null;
        // update properties
        if (updateLinkDto.Title != null) link.Title = updateLinkDto.Title;
        if (updateLinkDto.ExpiresAt.HasValue) link.ExpiresAt = updateLinkDto.ExpiresAt;
        if (updateLinkDto.IsActive.HasValue) link.IsActive = updateLinkDto.IsActive.Value;

        await linkRepo.UpdateShortLinkAsync(link);
        return LinkDto.FromEntity(link);
    }
}
