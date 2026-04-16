using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class AppUser : IdentityUser
{
    [MaxLength(100)]
    public string? DisplayName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
