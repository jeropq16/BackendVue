using _2_Domain.Entities;

namespace _2_Domain.Interfaces;

public interface IGalleryRepository
{
    Task<GalleryImage> AddAsync(GalleryImage image);
    Task<List<GalleryImage>> GetAllAsync();
    Task<GalleryImage?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}