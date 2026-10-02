using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusicaController : ControllerBase
{
    private readonly PrimataArchiveContext _context;

    public MusicaController(PrimataArchiveContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMusica()
    {
        var musicas = await _context.Musicas
            .Include(m => m.IdArtistaNavigation)
            .Include(m => m.IdAlbumNavigation)
            .Select(m => new MusicaResponse(
                m.IdMusica,
                m.IdArtista,
                m.IdArtistaNavigation != null ? m.IdArtistaNavigation.NomeArtista : null,
                m.IdAlbum,
                m.IdAlbumNavigation != null ? m.IdAlbumNavigation.Titulo : null,
                m.Titulo,
                m.Duracao,
                m.Genero,
                m.Capa,
                m.NumeroFaixa,
                m.DataLancamento,
                m.Reproducoes,
                m.DataCadastro
            ))
            .ToListAsync();

        return Ok(musicas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMusicaById(int id)
    {
        var musica = await _context.Musicas
            .Include(m => m.IdArtistaNavigation)
            .Include(m => m.IdAlbumNavigation)
            .FirstOrDefaultAsync(m => m.IdMusica == id);

        if (musica == null)
            return NotFound(new { Message = "Música não encontrada." });

        var response = new MusicaResponse(
            musica.IdMusica,
            musica.IdArtista,
            musica.IdArtistaNavigation?.NomeArtista,
            musica.IdAlbum,
            musica.IdAlbumNavigation?.Titulo,
            musica.Titulo,
            musica.Duracao,
            musica.Genero,
            musica.Capa,
            musica.NumeroFaixa,
            musica.DataLancamento,
            musica.Reproducoes,
            musica.DataCadastro
        );

        return Ok(response);
    }

    [HttpGet("album/{idAlbum}")]
    public async Task<IActionResult> GetMusicasPorAlbum(int idAlbum)
    {
        var musicas = await _context.Musicas
            .Where(m => m.IdAlbum == idAlbum)
            .Include(m => m.IdArtistaNavigation)
            .Include(m => m.IdAlbumNavigation)
            .Select(m => new MusicaResponse(
                m.IdMusica,
                m.IdArtista,
                m.IdArtistaNavigation != null ? m.IdArtistaNavigation.NomeArtista : null,
                m.IdAlbum,
                m.IdAlbumNavigation != null ? m.IdAlbumNavigation.Titulo : null,
                m.Titulo,
                m.Duracao,
                m.Genero,
                m.Capa,
                m.NumeroFaixa,
                m.DataLancamento,
                m.Reproducoes,
                m.DataCadastro
            ))
            .ToListAsync();

        return Ok(musicas);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarMusica([FromBody] CriarMusicaRequest request)
    {
        bool artistaExiste = await _context.Set<Artistum>().AnyAsync(a => a.IdArtista == request.IdArtista);
        if (!artistaExiste)
            return BadRequest(new { Message = "O artista informado não existe." });

        if (request.IdAlbum.HasValue)
        {
            bool albumExiste = await _context.Albums.AnyAsync(a => a.IdAlbum == request.IdAlbum.Value);
            if (!albumExiste)
                return BadRequest(new { Message = "O álbum informado não existe." });
        }

        var novaMusica = new Musica
        {
            IdArtista = request.IdArtista,
            IdAlbum = request.IdAlbum,
            Titulo = request.Titulo,
            Duracao = request.Duracao,
            Genero = request.Genero,
            Capa = request.Capa,
            NumeroFaixa = request.NumeroFaixa,
            DataLancamento = request.DataLancamento,
            Reproducoes = 0,
            DataCadastro = DateTime.UtcNow
        };

        _context.Musicas.Add(novaMusica);
        await _context.SaveChangesAsync();

        var response = new MusicaResponse(
            novaMusica.IdMusica,
            novaMusica.IdArtista,
            null,
            novaMusica.IdAlbum,
            null,
            novaMusica.Titulo,
            novaMusica.Duracao,
            novaMusica.Genero,
            novaMusica.Capa,
            novaMusica.NumeroFaixa,
            novaMusica.DataLancamento,
            novaMusica.Reproducoes,
            novaMusica.DataCadastro
        );

        return CreatedAtAction(nameof(GetMusicaById), new { id = novaMusica.IdMusica }, response);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> AtualizarMusica(int id, [FromBody] CriarMusicaRequest request)
    {
        var musica = await _context.Musicas.FindAsync(id);
        if (musica == null)
            return NotFound(new { Message = "Música não encontrada." });

        bool artistaExiste = await _context.Set<Artistum>().AnyAsync(a => a.IdArtista == request.IdArtista);
        if (!artistaExiste)
            return BadRequest(new { Message = "O artista informado não existe." });

        if (request.IdAlbum.HasValue)
        {
            bool albumExiste = await _context.Albums.AnyAsync(a => a.IdAlbum == request.IdAlbum.Value);
            if (!albumExiste)
                return BadRequest(new { Message = "O álbum informado não existe." });
        }

        musica.IdArtista = request.IdArtista;
        musica.IdAlbum = request.IdAlbum;
        musica.Titulo = request.Titulo;
        musica.Duracao = request.Duracao;
        musica.Genero = request.Genero;
        musica.Capa = request.Capa;
        musica.NumeroFaixa = request.NumeroFaixa;
        musica.DataLancamento = request.DataLancamento;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletarMusica(int id)
    {
        var musica = await _context.Musicas.FindAsync(id);
        if (musica == null)
            return NotFound(new { Message = "Música não encontrada." });

        _context.Musicas.Remove(musica);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}