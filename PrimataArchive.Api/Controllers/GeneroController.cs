using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeneroController : ControllerBase
{
    private readonly PrimataArchiveContext _context;

    public GeneroController(PrimataArchiveContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var generos = await _context.Generos
            .OrderBy(g => g.Nome)
            .Select(g => new GeneroResponse(g.IdGenero, g.Nome))
            .ToListAsync();

        return Ok(generos);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Criar([FromBody] CriarGeneroRequest request)
    {
        bool existe = await _context.Generos.AnyAsync(g => g.Nome == request.Nome);
        if (existe)
            return BadRequest(new { Message = "Este gênero já está cadastrado." });

        var genero = new Genero
        {
            Nome = request.Nome
        };

        _context.Generos.Add(genero);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterTodos), new GeneroResponse(genero.IdGenero, genero.Nome));
    }
}