using _1_Application.DTOs;
using _1_Application.Interfaces;
using _2_Domain.Entities;
using _2_Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace _1_Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICloudinaryService _cloudinaryService;

    public UserService(IUserRepository userRepository,  ICloudinaryService cloudinaryService)
    {
        _userRepository = userRepository;
        _cloudinaryService = cloudinaryService;
    }

    public Task<List<User>> GetAllAsync()
    {
        return _userRepository.GetAllAsync();
    }

    public Task<User?> GetByIdAsync(int id)
    {
        return _userRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var existing = await _userRepository.GetByIdAsync(user.Id);
        if (existing == null) return false;

        existing.Email = user.Email;
        existing.Role = user.Role;

        await _userRepository.UpdateAsync(existing);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _userRepository.GetByIdAsync(id);
        if (existing == null) return false;

        await _userRepository.DeleteAsync(id);
        return true;
        
    }
    
    public async Task UpdateMyProfileAsync(int userId, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("Usuario no encontrado.");

        if (!string.IsNullOrWhiteSpace(dto.Email))
            user.Email = dto.Email;

        if (dto.ProfilePhoto != null && dto.ProfilePhoto.Length > 0)
        {
            // borrar la foto pasada para no dar erro en cloud
            if (!string.IsNullOrWhiteSpace(user.ProfileImagePublicId))
                await _cloudinaryService.DeleteImageAsync(user.ProfileImagePublicId);

            await using var stream = dto.ProfilePhoto.OpenReadStream();
            var (url, publicId) = await _cloudinaryService.UploadImageAsync(stream, dto.ProfilePhoto.FileName);

            user.ProfileImageUrl = url;
            user.ProfileImagePublicId = publicId;
        }

        await _userRepository.UpdateAsync(user);
    }

    public async Task AdminUpdateUserPhotoAsync(int userId, IFormFile photo)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("Usuario no encontrado.");

        if (!string.IsNullOrWhiteSpace(user.ProfileImagePublicId))
            await _cloudinaryService.DeleteImageAsync(user.ProfileImagePublicId);

        await using var stream = photo.OpenReadStream();
        var (url, publicId) = await _cloudinaryService.UploadImageAsync(stream, photo.FileName);

        user.ProfileImageUrl = url;
        user.ProfileImagePublicId = publicId;

        await _userRepository.UpdateAsync(user);
    }
    
    
    
}