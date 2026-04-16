using Core.Entities;

namespace Core.Dtos;

public class LinkDto
{
    public int Id { get; set; }
    public required string ShortCode { get; set; }
    public required string OriginalUrl { get; set; }
    public string? Title { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public int ClickCount { get; set; }

    public static LinkDto FromEntity(ShortLink shortLink)
    {
        return new LinkDto
        {
            Id = shortLink.Id,
            ShortCode = shortLink.ShortCode,
            OriginalUrl = shortLink.OriginalUrl,
            Title = shortLink.Title,
            CreatedAt = shortLink.CreatedAt,
            ExpiresAt = shortLink.ExpiresAt,
            IsActive = shortLink.IsActive,
            ClickCount = 0
        };
    }
}
