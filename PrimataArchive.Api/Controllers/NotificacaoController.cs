using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificacaoController : ControllerBase
{
    private readonly PrimataArchiveContext _context;

    public NotificacaoController(PrimataArchiveContext context)
    {
        _context = context;
    }

    private int ObterUsuarioIdLogado()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out int id) ? id : 0;
    }

    [HttpGet]
    public async Task<IActionResult> ObterMinhasNotificacoes()
    {
        int usuarioId = ObterUsuarioIdLogado();

        var notificacoes = await _context.Notificacoes
            .Where(n => n.IdUsuario == usuarioId)
            .OrderByDescending(n => n.DataCriacao)
            .Select(n => new NotificacaoResponse(
                n.IdNotificacao,
                n.IdUsuario,
                n.Mensagem,
                n.Lida,
                n.DataCriacao
            ))
            .ToListAsync();

        return Ok(notificacoes);
    }

    [HttpPut("{id}/marcar-lida")]
    public async Task<IActionResult> MarcarComoLida(int id)
    {
        int usuarioId = ObterUsuarioIdLogado();

        var notificacao = await _context.Notificacoes
            .FirstOrDefaultAsync(n => n.IdNotificacao == id && n.IdUsuario == usuarioId);

        if (notificacao == null)
            return NotFound(new { Message = "Notificação não encontrada." });

        notificacao.Lida = true;
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Notificação marcada como lida." });
    }
}