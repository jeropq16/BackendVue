using Microsoft.AspNetCore.Http;

namespace _1_Application.DTOs;

public class UpdateUserPhotoDto
{
    public IFormFile ProfilePhoto { get; set; } = null!;

}