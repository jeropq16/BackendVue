using Microsoft.AspNetCore.Http;

namespace _1_Application.DTOs;

public class GalleryCreateDto
{
    public IFormFile File { get; set; } = default!;
    public string? Title { get; set; }
}