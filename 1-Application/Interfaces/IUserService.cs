using _1_Application.DTOs;
using _2_Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace _1_Application.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
    Task UpdateMyProfileAsync(int userId, UpdateUserDto dto);
    Task AdminUpdateUserPhotoAsync(int userId, IFormFile photo);
}