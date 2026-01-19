namespace _1_Application.DTOs;

public class GalleryItemDto
{
    public int Id { get; set; }
    public string Url { get; set; } = default!;
    public string? Title { get; set; }
    public DateTime CreatedAt { get; set; }
}