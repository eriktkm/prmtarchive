using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistController : ControlerBase
{
    private readonly PrimataArchiveContext _context;

    public PlaylistController(PrimataArchiveContext context)
    {
        _context = context;
    } 

    private int ObterUsuarioIdLogado()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.tryParse(idClaim, out int id) ? id : 0;
    }

    [HttpGet]
    public async Task<IActionResult> ObterPlaylistsPublicas()
    {
        var playlists = await _context.playlists
        .Include(p => p.IdUsuarioNavigation)
        .Include(p => p.PlaylistMusicas)
        .Where(p => p.Publica == true)
        .OrderByDescending(p => p.IdPlaylist)
        .Select(p => new PlaylistResponse(
            p.IdPlaylist,
            p.IdUsuario,
            p.IdUsuarioNavigation != null ? p.IdUsuarioNavigation.Nome : null,
            p.Nome,
            p.Descricao,
            p.Capa,
            p.Publica,
            p.PlaylistMusicas.Count,
            p.DataCriacao
        ))
        .ToListAsync();

        return Ok(playlists);
    }

    [HttpGet("minhas")]
    [Authorize]
    public async Task<IActionResult> ObterMinhasPlaylists()
    {
        int usuarioId = ObterUsuarioIdLogado();

        var playlists = await _context.Playlists
        .Include(p => p.IdUsuarioNavigation)
            .Include(p => p.PlaylistMusicas)
            .Where(p => p.IdUsuario == usuarioId)
            .OrderByDescending(p => p.IdPlaylist)
            .Select(p => new PlaylistResponse(
                p.IdPlaylist,
                p.IdUsuario,
                p.IdUsuarioNavigation != null ? p.IdUsuarioNavigation.Nome : null,
                p.Nome,
                p.Descricao,
                p.Capa,
                p.Publica,
                p.PlaylistMusicas.Count,
                p.DataCriacao
            ))
            .ToListAsync();

            return Ok(playlists);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var playlist = await _context.Playlists
        .Include(p => p.IdUsuarioNavigation)
        .Include(p => p.PlaylistMusicas)
        .FirstOrDefaultAsync(p => p.IdPlaylist == id);

        if(playlist == null)
        return NotFound(new {Message = "Playlist não encontrada."});

        int usuarioIdLogado = ObterUsuarioIdLogado();
        if(playlist.Publica == false && playlist.IdUsuario != usuarioIdLogado)
        return Forbid();

        var response = new PlaylistResponse(
            playlist.IdPlaylist,
            playlist.IdUsuario,
            playlist.IdUsuarioNavigation?.Nome,
            playlist.Nome,
            playlist.Descricao,
            playlist.Capa,
            playlist.Publica,
            playlist.PlaylistMusicas.Count,
            playlist.DataCriacao
        );

        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Criar([FromBody] CriarPlaylist request)
    {
        int usuarioId = ObterUsuarioIdLogado();

        var playlist = new Playlist
        {
            IdUsuario = usuarioId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Capa = request.Capa,
            Publica = request.Publica,
            DataCriacao = DateTime.UtcNow
        };

        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();

        var response = new PlaylistResponse
        (
            playlist.IdPlaylist,
            playlist.IdUsuario,
            User.Identity?.Name,
            playlist.Nome,
            playlist.Descricao,
            playlist.Capa,
            playlist.Publica,
            0,
            playlist.DataCriacao
        );

        return CreatedAction(nameof(ObterPorId), new {id = playlist.IdPlaylist}, response);
    }

    [HttpPost("{id}/musicas")]
    [Authorize]
    public async Task<IActionResult> AdicionarMusica(int id, [FromBody] AdicionarMusicaPlaylistRequest request)
    {
        int usuarioId = ObterUsuarioIdLogado();

        var playlist = await _context.Playlists.FirstOrDefaultAsync(p => p.IdPlaylist == id && p.IdUsuario == usuarioId);
        if(playlist == null)
        return NotFound(new {Message = "Playlist não encontrada ou não pertence ao usuário."})

        bool musicaExiste = await _context.Musicas.AnyAsync(m => m.IdMusica == request.IdMusica);
        if(!musicaExiste)
        return NotFound(new {Message = "Musica não encontrada."})

        bool jaExiste = await _context.PlaylistMusicas
        .AnyAsync(pm => pm.IdPlaylist == id && pm.IdMusica == request.IdMusica);

        if(jaExiste)
        return BadRequest(new {Message = "Essa música já está presente na playlist."});

        var playlistMusica = new PlaylistMusica
        {
            IdPlaylist = id,
            IdMusica = requesr.IdMusica,
            DataAdicao = DateTime.UtcNow
        };

        _context.PlaylistMusicas.Add(playlistMusica);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Música adicionada à playlist com sucesso." });
    }

    [HttpDelete("{id}/musicas/{idMusica}")]
    [Authorize]
    public async Task<IActionResult> RemoverMusica(int id, int idMusica)
    {
        int usuarioId = ObterUsuarioIdLogado();

        var playlist = await _context.Playlists.FirstOrDefaultAsync(p => p.IdPlaylist == id && p.IdUsuario == usuarioId);
        if (playlist == null)
            return NotFound(new { Message = "Playlist não encontrada ou não pertence ao usuário." });

        var item = await _context.PlaylistMusicas
            .FirstOrDefaultAsync(pm => pm.IdPlaylist == id && pm.IdMusica == idMusica);

        if (item == null)
            return NotFound(new { Message = "Música não encontrada nesta playlist." });

        _context.PlaylistMusicas.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Deletar(int id)
    {
        int usuarioId = ObterUsuarioIdLogado();

        var playlist = await _context.Playlists.FirstOrDefaultAsync(p => p.IdPlaylist == id && p.IdUsuario == usuarioId);
        if (playlist == null)
            return NotFound(new { Message = "Playlist não encontrada ou não pertence ao usuário." });

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    
}