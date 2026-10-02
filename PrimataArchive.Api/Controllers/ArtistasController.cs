using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistasController : ControllerBase
{
    private readonly PrimataArchiveContext _context;

    public ArtistasController(PrimataArchiveContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetArtistas()
    {
        var artistas = await _context.Set<Artistum>()
            .Select(a => new ArtistaResponse(
                a.IdArtista,
                a.NomeArtista,
                a.Foto,
                a.DataCadastro
            ))
            .ToListAsync();

        return Ok(artistas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetArtistaById(int id)
    {
        var artista = await _context.Set<Artistum>()
            .FirstOrDefaultAsync(a => a.IdArtista == id);

        if (artista == null)
            return NotFound(new { Message = "Artista não encontrado." });

        var response = new ArtistaResponse(
            artista.IdArtista,
            artista.NomeArtista,
            artista.Foto,
            artista.DataCadastro
        );

        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarArtista([FromBody] ArtistaRequest request)
    {
        var novoArtista = new Artistum
        {
            NomeArtista = request.NomeArtista,
            Foto = request.Foto,
            DataCadastro = DateTime.UtcNow
        };

        _context.Set<Artistum>().Add(novoArtista);
        await _context.SaveChangesAsync();

        var response = new ArtistaResponse(
            novoArtista.IdArtista,
            novoArtista.NomeArtista,
            novoArtista.Foto,
            novoArtista.DataCadastro
        );

        return CreatedAtAction(nameof(GetArtistaById), new { id = novoArtista.IdArtista }, response);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> AtualizarArtista(int id, [FromBody] ArtistaRequest request)
    {
        var artista = await _context.Set<Artistum>().FindAsync(id);
        if (artista == null)
            return NotFound(new { Message = "Artista não encontrado." });

        artista.NomeArtista = request.NomeArtista;
        artista.Foto = request.Foto;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletarArtista(int id)
    {
        var artista = await _context.Set<Artistum>().FindAsync(id);
        if (artista == null)
            return NotFound(new { Message = "Artista não encontrado." });

        _context.Set<Artistum>().Remove(artista);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}