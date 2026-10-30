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
            .Include(b => b.IdMusicaNavigation)
            .Where(b => b.IdUsuario == usuarioId)
            .OrderByDescending(b => b.DataAdicao)
            .Select(b => new BibliotecaResponse(
                b.IdBiblioteca,
                b.IdUsuario,
                b.IdMusica,
                b.IdMusicaNavigation != null ? b.IdMusicaNavigation.Titulo : null,
                b.DataAdicao
            ))
            .ToListAsync();
        return Ok(itens);
    }
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarBibliotecaRequest request)
    {
        int usuarioId = ObterUsuarioIdLogado();

        if (request.IdMusica <= 0)
            return BadRequest(new { Message = "Informe uma Música válida para adicionar à biblioteca." });

        var jaExiste = await _context.Bibliotecas
            .AnyAsync(b => b.IdUsuario == usuarioId && b.IdMusica == request.IdMusica);

        if (jaExiste)
            return BadRequest(new { Message = "Esta música já está na sua biblioteca." });

        var item = new Biblioteca
        {
            IdUsuario = usuarioId,
            IdMusica = request.IdMusica,
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

// DTOs/Records caso estejam neste mesmo ficheiro ou precisem ser atualizados
public record BibliotecaResponse(
    int IdBiblioteca,
    int IdUsuario,
    int IdMusica,
    string? TituloMusica,
    DateTime? DataAdicao
);

public record AdicionarBibliotecaRequest(
    int IdMusica
);