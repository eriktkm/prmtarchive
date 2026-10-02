using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;
using PrimataArchive.Api.Services;
using BCrypt.Net;

namespace PrimataArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly PrimataArchiveContext _context;
    private readonly TokenService _tokenService;

    public AuthController(PrimataArchiveContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarRequest request)
    {
        bool eMailExistente = await _context.Usuarios.AnyAsync(u => u.Email == request.Email);
        if (eMailExistente)
        {
            return BadRequest(new { Message = "Este e-mail já está cadastrado." });
        }

        string senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

        var novoUsuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            Senha = senhaHash
        };

        _context.Usuarios.Add(novoUsuario);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Usuário registrado com sucesso." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.Senha))
        {
            return Unauthorized(new { Message = "E-mail ou senha inválidos." });
        }

        var token = _tokenService.GenerateToken(usuario);

        return Ok(new
        {
            mensagem = "Login bem-sucedido",
            usuarioId = usuario.IdUsuario,
            nome = usuario.Nome,
            token = token,
            email = usuario.Email
        });
    }
}