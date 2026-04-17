using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class ShortLink : BaseEntity
{
    [MaxLength(10)]
    public required string ShortCode { get; set; }
    [MaxLength(2048)]
    public required string OriginalUrl { get; set; }
    [MaxLength(200)]
    public string? Title { get; set; }
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public DateTimeOffset? ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<ClickEvent> ClickEvents { get; set; } = [];
}
