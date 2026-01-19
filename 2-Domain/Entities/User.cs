using _2_Domain.Enums;

namespace _2_Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Role  Role { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? ProfileImagePublicId { get; set; }
}