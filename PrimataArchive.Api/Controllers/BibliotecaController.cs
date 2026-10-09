using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BibliotecaController : ControllerBase
{
    private readonly PrimataArchiveContext _context;

    public BibliotecaController(PrimataArchiveContext context)
    {
        _context = context;
    }

    private int ObterUsuarioIdLogado()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out int id) ? id : 0;
    }

    [HttpGet]
    public async Task<IActionResult> ObterMinhaBiblioteca()
    {
        int usuarioId = ObterUsuarioIdLogado();

        var itens = await _context.Bibliotecas
            .Include(b => b.IdAlbumNavigation)
            .Include(b => b.IdPlaylistNavigation)
            .Where(b => b.IdUsuario == usuarioId)
            .OrderByDescending(b => b.DataAdicao)
            .Select(b => new BibliotecaResponse(
                b.IdBiblioteca,
                b.IdUsuario,
                b.IdAlbum,
                b.IdAlbumNavigation != null ? b.IdAlbumNavigation.Titulo : null,
                b.IdPlaylist,
                b.IdPlaylistNavigation != null ? b.IdPlaylistNavigation.Nome : null,
                b.DataAdicao
            ))
            .ToListAsync();

        return Ok(itens);
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarBibliotecaRequest request)
    {
        int usuarioId = ObterUsuarioIdLogado();

        if (!request.IdAlbum.HasValue && !request.IdPlaylist.HasValue)
            return BadRequest(new { Message = "Informe um Álbum ou uma Playlist para adicionar à biblioteca." });

        var item = new Biblioteca
        {
            IdUsuario = usuarioId,
            IdAlbum = request.IdAlbum,
            IdPlaylist = request.IdPlaylist,
            DataAdicao = DateTime.UtcNow
        };

        _context.Bibliotecas.Add(item);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Item adicionado à biblioteca com sucesso.", IdBiblioteca = item.IdBiblioteca });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        int usuarioId = ObterUsuarioIdLogado();

        var item = await _context.Bibliotecas
            .FirstOrDefaultAsync(b => b.IdBiblioteca == id && b.IdUsuario == usuarioId);

        if (item == null)
            return NotFound(new { Message = "Item não encontrado na biblioteca." });

        _context.Bibliotecas.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}