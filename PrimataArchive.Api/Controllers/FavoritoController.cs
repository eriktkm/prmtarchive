using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoritoController : ControllerBase
    {
        private readonly PrimataArchiveContext _context;

        public FavoritoController(PrimataArchiveContext context)
    {
        _context = context;
    }

    private int ObterUsuarioIdLogado()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out int id) ? id : 0;
    }

    [HttpGet("meus")]
    public async Task<IActionResult> ObterMeusFavoritos()
    {
        int usuarioId = ObterUsuarioIdLogado();

        var favoritos = await _context.Favoritos
        .Where(f => f.IdUsuario == usuarioId)
        .Include(f => f.IdAlbumNavigation)
        .ThenInclude(a => a.IdArtistaNavigation)
        .Select(f => new FavoritoResponse(
            f.IdFavorito,
            f.IdUsuario,
            f.IdAlbum,
            f.IdAlbumNavigation.Titulo,
            f.IdAlbumNavigation.Capa,
            f.IdAlbumNavigation.IdArtistaNavigation != null ? f.IdAlbumNavigation.IdArtistaNavigation.NomeArtista : null,
            f.DataFavorito
        ))
        .ToListAsync();
        return Ok(favoritos);
    }

    [HttpGet("verificar/{idAlbum}")]
    public async Task<IActionResult> VerificarFavorito(int idAlbum)
    {
        int usuarioId = ObterUsuarioIdLogado();
        bool ehFavorito = await _context.Favoritos
        .AnyAsync(f => f.IdUsuario == usuarioId && f.IdAlbum == idAlbum);

        return Ok(new {EhFavorito = ehFavorito});
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarFavorito([FromBody] AdicionarFavoritoRequest request)
    {
        int usuarioId = ObterUsuarioIdLogado();

        bool albumExiste = await _context.Set<Album>().AnyAsync(a => a.IdAlbum == request.IdAlbum);
        if(!albumExiste)
        return BadRequest(new {Message = "O album informado não existe."});

        bool jaFavoritou = await _context.Favoritos
        .AnyAsync(f => f.IdUsuario == usuarioId && f.IdAlbum == request.IdAlbum);

        if(jaFavoritou)
        return BadRequest(new {Message = "Este álbum já está nos seus favoritos."});

        var favorito = new Favorito
        {
            IdUsuario = usuarioId,
            IdAlbum = request.IdAlbum,
            DataFavorito = DateTime.UtcNow
        };

        _context.Favoritos.Add(favorito);
        await _context.SaveChangesAsync();

        return Ok(new {Message = "Álbum adicionado aos favoritos.", IdFavorito = favorito.IdFavorito});
    }

    [HttpDelete("album/{idAlbum}")]
    public async Task<IActionResult> RemoverFavorito(int idAlbum)
    {
        int usuarioId = ObterUsuarioIdLogado();

        var favorito = await _context.Favoritos
        .FirstOrDefaultAsync(f => f.IdUsuario == usuarioId && f.IdAlbum == idAlbum);

        if(favorito == null)
        return NotFound(new {Message = "Favorito não encontrado."});

        _context.Favoritos.Remove(favorito);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    }