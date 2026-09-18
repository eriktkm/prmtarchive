using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsuariosController : ControllerBase
{
    private readonly PrimataArchiveContext _context;
    public UsuariosController(PrimataArchiveContext context)
    {
        _context = context;
    }

[HttpGet]
public async Task<IActionResult> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUsuarioPorId(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound(new { Message = "Usuario no encontrado" });
        }

        return Ok(usuario);
    }
}