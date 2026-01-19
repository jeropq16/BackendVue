using _1_Application.Interfaces;
using _2_Domain.Entities;
using _2_Domain.Interfaces;

namespace _1_Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
    
    
    
}