using Core.Dtos;

namespace Core.Interfaces;

public interface IShortLinkService
{
    Task<LinkDto> CreateShortLinkAsync(CreateLinkDto createLinkDto, string userId);
    Task<IList<LinkDto>> GetListOfShortLinksAsync(string userId);
    Task<LinkDto?> GetShortLinkAsync(int id, string userId);
    Task<LinkDto?> UpdateLinkAsync(int id, UpdateLinkDto updateLinkDto, string userId);
    Task<bool> DeleteShortLinkAsync(int id, string userId);
    Task<string?> ResolveAndTrackAsync(string shortCode, string? ipAddress, string? userAgent, string? referrer);
}