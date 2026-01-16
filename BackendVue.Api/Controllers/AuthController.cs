using _1_Application.DTOs;
using _1_Application.Interfaces;
using _2_Domain.Entities;
using _2_Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendVue.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController  : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthController(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) return Unauthorized();

        var valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!valid) return Unauthorized();

        var token = _jwtService.GenerateToken(user);

        return Ok(new
        {
            token,
            userId = user.Id,
            role = user.Role.ToString()
        });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var exists = await _userRepository.ExistsByEmailAsync(request.Email);
        if (exists)
            return BadRequest("Email already registered");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role
        };

        await _userRepository.AddAsync(user);

        return Ok();
    }
}