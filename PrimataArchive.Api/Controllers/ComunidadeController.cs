using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class ComunidadeController : ControllerBase
    {
        private readonly PrimataArchiveContext _context;

        public ComunidadeController(PrimataArchiveContext context)
    {
        _context = context;
    }

    private int ObterUsuarioIdLogado()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out int id) ? id : 0;
    }

    [HttpGet]
    public async Task<IActionResult> ObterComunidades()
    {
        var comunidades = await _context.Comunidades
        .Include(c => c.IdUsuarioCriadorNavigation)
        .Include(c => c.ComunidadeMembros)
        .Select(c => new ComunidadeResponse (
            c.IdComunidade,
            c.IdUsuarioCriador,
            c.IdUsuarioCriadorNavigation != null ? c.IdUsuarioCriadorNavigation.Nome : null,
            c.Nome,
            c.Descricao,
            c.Foto,
            c.ComunidadeMembros.Count,
            c.DataCriacao
        ))
        .ToListAsync();

        return Ok(comunidades);
    }

    [HttpGet ("{id}")]
    public async Task<IActionResult> ObterComunidadePorId (int id)
    {
        var comunidade = await _context.Comunidades
        .Include(c => c.IdUsuarioCriadorNavigation)
        .Include(c => c.ComunidadeMembros)
        .FirstOrDefaultAsync(c => c.IdComunidade == id);

        if(comunidade == null)
        return NotFound(new {Message = "Comunidade não encontrada"});

        int usuarioIdLogado = ObterUsuarioIdLogado();
        bool ehMembro = comunidade.ComunidadeMembros.Any(cm => cm.IdUsuario == usuarioIdLogado);

        return Ok(new
        {
         Comunidade = new ComunidadeResponse(
            comunidade.IdComunidade,
            comunidade.IdUsuarioCriador,
            comunidade.IdUsuarioCriadorNavigation?.Nome,
            comunidade.Nome,
            comunidade.Descricao,
            comunidade.Foto,
            comunidade.ComunidadeMembros.Count,
            comunidade.DataCriacao
         ),
         EhMembro = ehMembro    
        }
        );
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarComunidade([FromBody] CriarComunidadeRequest request)
    {
        int usuarioId = ObterUsuarioIdLogado();

        bool nomeExiste = await _context.Comunidades.AnyAsync(c => c.Nome == request.Nome);
        if(nomeExiste)
        return BadRequest(new {Message = "Já existe uma comunidade com esse nome."});
        
        var comunidade = new Comunidade
        {
            IdUsuarioCriador = usuarioId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Foto = request.Foto,
            DataCriacao = DateTime.UtcNow
        };

        _context.Comunidades.Add(comunidade);
        await _context.SaveChangesAsync();

        var membroCriador = new ComunidadeMembro
        {
            IdComunidade = comunidade.IdComunidade,
            IdUsuario = usuarioId,
            DataIngresso = DateTime.UtcNow
        };

        _context.ComunidadeMembros.Add(membroCriador);
        await _context.SaveChangesAsync();

        var response = new ComunidadeResponse(
            comunidade.IdComunidade,
            comunidade.IdUsuarioCriador,
            User.Identity?.Name,
            comunidade.Nome,
            comunidade.Descricao,
            comunidade.Foto,
            1,
            comunidade.DataCriacao
        );

        return CreatedAtAction(nameof (ObterComunidadePorId), new {id = comunidade.IdComunidade}, response);
    }

    [HttpPost("{id}/entrar")]
    [Authorize]
    public async Task<IActionResult> EntrarNaComunidade(int id)
    {
        int usuarioId = ObterUsuarioIdLogado();

        var comunidadeExiste = await _context.Comunidades.AnyAsync(c => c.IdComunidade == id);
        if(!comunidadeExiste)
        return NotFound (new {Message = "Comunidade não encontrada."});

        bool jaEhMembro = await _context.ComunidadeMembros
        .AnyAsync (cm => cm.IdComunidade == id && cm.IdUsuario == usuarioId);

        if(jaEhMembro)
        return BadRequest(new {Message = "Você já é membro dessa comunidade."});

        var membro = new ComunidadeMembro
        {
            IdComunidade = id,
            IdUsuario = usuarioId,
            DataIngresso = DateTime.UtcNow
        };

        _context.ComunidadeMembros.Add(membro);
        await _context.SaveChangesAsync();

        return Ok(new {Message = "Você entrou na comunidade com sucesso."});
    }

    }