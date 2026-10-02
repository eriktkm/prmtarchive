using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController : ControllerBase
{
    private readonly PrimataArchiveContext _context;

    public AlbumController(PrimataArchiveContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAlbum()
    {
        var albuns = await _context.Albums
            .Include(a => a.IdArtistaNavigation)
            .Select(a => new AlbumResponse(
                a.IdAlbum,
                a.IdArtista,
                a.IdArtistaNavigation != null ? a.IdArtistaNavigation.NomeArtista : null,
                a.Titulo,
                a.Capa ?? "",
                a.Descricao,
                a.Genero,
                a.DataLancamento,
                a.Tipo,
                a.DataCadastro
            ))
            .ToListAsync();

        return Ok(albuns);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAlbumById(int id)
    {
        var album = await _context.Albums
            .Include(a => a.IdArtistaNavigation)
            .FirstOrDefaultAsync(a => a.IdAlbum == id);

        if (album == null)
            return NotFound(new { Message = "Álbum não encontrado." });

        var response = new AlbumResponse(
            album.IdAlbum,
            album.IdArtista,
            album.IdArtistaNavigation?.NomeArtista,
            album.Titulo,
            album.Capa ?? "",
            album.Descricao,
            album.Genero,
            album.DataLancamento,
            album.Tipo,
            album.DataCadastro
        );

        return Ok(response);
    }

    [HttpGet("artista/{idArtista}")]
    public async Task<IActionResult> GetAlbunsPorArtista(int idArtista)
    {
        var albuns = await _context.Albums
            .Where(a => a.IdArtista == idArtista)
            .Include(a => a.IdArtistaNavigation)
            .Select(a => new AlbumResponse(
                a.IdAlbum,
                a.IdArtista,
                a.IdArtistaNavigation != null ? a.IdArtistaNavigation.NomeArtista : null,
                a.Titulo,
                a.Capa ?? "",
                a.Descricao,
                a.Genero,
                a.DataLancamento,
                a.Tipo,
                a.DataCadastro
            ))
            .ToListAsync();

        return Ok(albuns);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarAlbum([FromBody] AlbumRequest request)
    {
        bool artistaExiste = await _context.Set<Artistum>().AnyAsync(a => a.IdArtista == request.IdArtista);
        if (!artistaExiste)
            return BadRequest(new { Message = "O artista informado não existe." });

        var novoAlbum = new Album
        {
            IdArtista = request.IdArtista,
            Titulo = request.Titulo,
            Capa = request.Capa,
            Descricao = request.Descricao,
            Genero = request.Genero,
            DataLancamento = request.DataLancamento,
            Tipo = request.Tipo,
            DataCadastro = DateTime.UtcNow
        };

        _context.Albums.Add(novoAlbum);
        await _context.SaveChangesAsync();

        var response = new AlbumResponse(
            novoAlbum.IdAlbum,
            novoAlbum.IdArtista,
            null,
            novoAlbum.Titulo,
            novoAlbum.Capa ?? "",
            novoAlbum.Descricao,
            novoAlbum.Genero,
            novoAlbum.DataLancamento,
            novoAlbum.Tipo,
            novoAlbum.DataCadastro
        );

        return CreatedAtAction(nameof(GetAlbumById), new { id = novoAlbum.IdAlbum }, response);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> AtualizarAlbum(int id, [FromBody] AlbumRequest request)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null)
            return NotFound(new { Message = "Álbum não encontrado." });

        bool artistaExiste = await _context.Set<Artistum>().AnyAsync(a => a.IdArtista == request.IdArtista);
        if (!artistaExiste)
            return BadRequest(new { Message = "O artista informado não existe." });

        album.IdArtista = request.IdArtista;
        album.Titulo = request.Titulo;
        album.Capa = request.Capa;
        album.Descricao = request.Descricao;
        album.Genero = request.Genero;
        album.DataLancamento = request.DataLancamento;
        album.Tipo = request.Tipo;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletarAlbum(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null)
            return NotFound(new { Message = "Álbum não encontrado." });

        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}