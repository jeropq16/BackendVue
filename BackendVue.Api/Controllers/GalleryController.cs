using _1_Application.DTOs;
using _1_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendVue.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GalleryController  : ControllerBase
{
    private readonly GalleryService _service;

    public GalleryController(GalleryService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize] 
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromForm] GalleryCreateDto dto)
    {
        if (dto.File == null || dto.File.Length == 0) return BadRequest("Archivo inválido.");

        var created = await _service.CreateAsync(dto);
        return Ok(created);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return Ok(ok);
    }
}