using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class ClickEvent : BaseEntity
{
    public required int ShortLinkId { get; set; }
    public required ShortLink ShortLink { get; set; }
    [MaxLength(2048)]
    public string? Referrer { get; set; }
    [MaxLength(200)]
    public string? UserAgent { get; set; }
    [MaxLength(45)]
    public string? IpAddress { get; set; }
}
