using _1_Application.DTOs;
using _1_Application.Interfaces;
using _2_Domain.Entities;
using _2_Domain.Interfaces;

namespace _1_Application.Services;

public class GalleryService
{
    private readonly IGalleryRepository _repo;
    private readonly ICloudinaryService _cloudinary;

    public GalleryService(IGalleryRepository repo, ICloudinaryService cloudinary)
    {
        _repo = repo;
        _cloudinary = cloudinary;
    }

    public async Task<GalleryItemDto> CreateAsync(GalleryCreateDto dto)
    {
        await using var stream = dto.File.OpenReadStream();
        var (url, publicId) = await _cloudinary.UploadImageAsync(stream, dto.File.FileName);

        var entity = new GalleryImage
        {
            Url = url,
            PublicId = publicId,
            Title = dto.Title
        };

        var save = await _repo.AddAsync(entity);

        return new GalleryItemDto
        {
            Id = save.Id,
            Url = save.Url,
            Title = save.Title,
            CreatedAt = save.CreatedAt
        };
    }

    public async Task<List<GalleryItemDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(i => new GalleryItemDto
        {
            Id = i.Id,
            Url = i.Url,
            Title = i.Title,
            CreatedAt = i.CreatedAt
        }).ToList();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var img = await _repo.GetByIdAsync(id);
        if (img is null) return false;

        await _cloudinary.DeleteImageAsync(img.PublicId);
        return await _repo.DeleteAsync(id);
    }
}