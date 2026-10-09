using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DenunciaController : ControllerBase
{
    private readonly PrimataArchiveContext _context;

    public DenunciaController(PrimataArchiveContext context)
    {
        _context = context;
    }

    private int ObterUsuarioIdLogado()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out int id) ? id : 0;
    }

    [HttpPost]
    public async Task<IActionResult> CriarDenuncia([FromBody] CriarDenunciaRequest request)
    {
        int usuarioId = ObterUsuarioIdLogado();

        var denuncia = new Denuncium
        {
            IdUsuario = usuarioId,
            Motivo = request.Motivo,
            Descricao = request.Descricao,
            IdPost = request.IdPost,
            IdComentario = request.IdComentario,
            IdUsuarioDenunciado = request.IdUsuarioDenunciado,
            Status = "Pendente",
            DataCriacao = DateTime.UtcNow
        };

        _context.Denuncia.Add(denuncia);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Denúncia registrada com sucesso.", IdDenuncia = denuncia.IdDenuncia });
    }

    [HttpGet]
    public async Task<IActionResult> ObterMinhasDenuncias()
    {
        int usuarioId = ObterUsuarioIdLogado();

        var denuncias = await _context.Denuncia
            .Where(d => d.IdUsuario == usuarioId)
            .OrderByDescending(d => d.DataCriacao)
            .Select(d => new DenunciaResponse(
                d.IdDenuncia,
                d.IdUsuario,
                d.Motivo,
                d.Descricao,
                d.Status,
                d.DataCriacao
            ))
            .ToListAsync();

        return Ok(denuncias);
    }
}