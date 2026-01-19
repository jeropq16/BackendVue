namespace _2_Domain.Entities;

public class GalleryImage
{
    public int Id { get; set; }
    public string Url { get; set; } = default!;
    public string PublicId { get; set; } = default!;
    public string? Title { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}