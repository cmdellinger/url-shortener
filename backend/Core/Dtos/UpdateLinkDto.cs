namespace Core.Dtos;

public class UpdateLinkDto
{
    public string? Title { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public bool? IsActive { get; set; }
}
