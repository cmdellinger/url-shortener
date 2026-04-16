using System;

namespace Core.Dtos;

public class CreateLinkDto
{
    public required string OriginalUrl { get; set; }
    public string? Title { get; set; }
}