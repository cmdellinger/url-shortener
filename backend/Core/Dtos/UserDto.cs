using Core.Entities;

namespace Core.Dtos;

public class UserDto
{
    public required string Id { get; set; } // identity's user id is a string
    public required string Email { get; set; } = null!;
    public string? DisplayName { get; set; } = null!;

    public static UserDto FromEntity(AppUser user)
    {
        return new UserDto
        {
          Id = user.Id,
          Email = user.Email!,
          DisplayName = user.DisplayName
        };
    }
}
