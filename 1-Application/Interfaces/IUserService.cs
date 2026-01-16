using _2_Domain.Entities;

namespace _1_Application.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
}