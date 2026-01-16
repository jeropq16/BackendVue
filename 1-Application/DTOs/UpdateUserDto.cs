using _2_Domain.Enums;

namespace _1_Application.DTOs;

public class UpdateUserDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Role? Role { get; set; }
}