using _2_Domain.Enums;

namespace _1_Application.DTOs;

public class RegisterRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Role  Role { get; set; } 
}