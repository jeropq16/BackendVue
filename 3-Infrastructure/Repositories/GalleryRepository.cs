using _2_Domain.Entities;
using _2_Domain.Interfaces;
using _3_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace _3_Infrastructure.Repositories;

public class GalleryRepository : IGalleryRepository
{
    private readonly AppDbContext _context;

    public GalleryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GalleryImage> AddAsync(GalleryImage image)
    {
        _context.GalleryImages.Add(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task<List<GalleryImage>> GetAllAsync()
        => await _context.GalleryImages.OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

    public async Task<GalleryImage?> GetByIdAsync(int id)
        => await _context.GalleryImages.FirstOrDefaultAsync(i => i.Id == id);

    public async Task<bool> DeleteAsync(int id)
    {
        var img = await _context.GalleryImages.FirstOrDefaultAsync(i => i.Id == id);
        if (img is null) return false;

        _context.GalleryImages.Remove(img);
        await _context.SaveChangesAsync();
        return true;
    }
}