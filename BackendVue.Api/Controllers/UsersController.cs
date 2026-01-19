using System.Security.Claims;
using _1_Application.DTOs;
using _1_Application.Interfaces;
using _2_Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendVue.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ICloudinaryService  _cloudinaryService;

    public UsersController(IUserRepository userRepository,  ICloudinaryService cloudinaryService)
    {
        _userRepository = userRepository;
        _cloudinaryService = cloudinaryService;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepository.GetAllAsync();

        var result = users.Select(u => new
        {
            u.Id,
            u.Email,
            role = u.Role.ToString(),
            profileImageUrl = u.ProfileImageUrl
        });

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();

        return Ok(new
        {
            user.Id,
            user.Email,
            role = user.Role.ToString(),
            profileImageUrl = user.ProfileImageUrl
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Email))
            user.Email = dto.Email;

        if (dto.Role.HasValue)
            user.Role = dto.Role.Value;
        
        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        await _userRepository.UpdateAsync(user);

        return Ok(new
        {
            user.Id,
            user.Email,
            role = user.Role.ToString()
        });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();

        await _userRepository.DeleteAsync(id);
        return NoContent();
    }
    
    //Foto cloudinary
     [Authorize]
     [HttpPut("me")]
     [Consumes("multipart/form-data")]
     public async Task<IActionResult> UpdateMe([FromForm] UpdateMyProfileDto dto)
     {
         var userIdClaim = User.FindFirst("userId")?.Value;
         if (string.IsNullOrWhiteSpace(userIdClaim)) return Unauthorized();
     
         var userId = int.Parse(userIdClaim);
     
         var user = await _userRepository.GetByIdAsync(userId);
         if (user == null) return NotFound();
     
         if (!string.IsNullOrWhiteSpace(dto.Email))
             user.Email = dto.Email;
     
         if (!string.IsNullOrWhiteSpace(dto.Password))
             user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
     
         if (dto.ProfilePhoto != null && dto.ProfilePhoto.Length > 0)
         {
             if (!string.IsNullOrWhiteSpace(user.ProfileImagePublicId)) 
                 await _cloudinaryService.DeleteImageAsync(user.ProfileImagePublicId);
     
             await using var stream = dto.ProfilePhoto.OpenReadStream();
             var upload = await _cloudinaryService.UploadImageAsync(stream, dto.ProfilePhoto.FileName);
     
             user.ProfileImageUrl = upload.Url;
             user.ProfileImagePublicId = upload.PublicId;
         }
     
         await _userRepository.UpdateAsync(user);
     
         return Ok(new
         {
             user.Id,
             user.Email,
             role = user.Role.ToString(),
             profileImageUrl = user.ProfileImageUrl
         });
     }
     
     
     
     // ADMIN
     [Authorize(Roles = "Admin")]
     [HttpPut("{id:int}/photo")]
     [Consumes("multipart/form-data")]
     public async Task<IActionResult> AdminUpdatePhoto(int id, [FromForm] UpdateUserPhotoDto dto)
     {
         var user = await _userRepository.GetByIdAsync(id);
         if (user == null) return NotFound();

         if (!string.IsNullOrWhiteSpace(user.ProfileImagePublicId))
             await _cloudinaryService.DeleteImageAsync(user.ProfileImagePublicId);

         await using var stream = dto.ProfilePhoto.OpenReadStream();
         var upload = await _cloudinaryService.UploadImageAsync(stream, dto.ProfilePhoto.FileName);

         user.ProfileImageUrl = upload.Url;
         user.ProfileImagePublicId = upload.PublicId;

         await _userRepository.UpdateAsync(user);

         return Ok(new
         {
             user.Id,
             user.Email,
             role = user.Role.ToString(),
             profileImageUrl = user.ProfileImageUrl
         });
     }

}