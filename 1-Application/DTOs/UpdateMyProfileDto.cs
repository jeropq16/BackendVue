using Microsoft.AspNetCore.Http;

namespace _1_Application.DTOs;

public class UpdateMyProfileDto
{
    public string? Email { get; set; }
    public string? Password { get; set; } 
    public IFormFile? ProfilePhoto { get; set; }
}