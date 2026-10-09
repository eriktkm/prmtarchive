using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly  PrimataArchiveContext _context;

    public PostController(PrimataArchive context)
    {
        _context = context;
    }

    private int ObterUsuarioIdLogado()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return int.tryParse(idClaim, out int id) ? id : 0;
    }

    [HttpGet]
    public async Task<IActionResult> ObterPosts([FromQuery] int? idComunidade)
    {
        var query = _context.Posts.AsQueryable();

        if(idComunidade.HasValue)
        query = query.Where(p => p.idComunidade == idComunidade.Value);

        var posts = await query
            .Include(p => p.IdUsuarioNavigation)
            .Include(p => p.IdComunidadeNavigation)
            .Include(p => p.Comentarios)
            .OrderByDescending(p => p.IdPost)
            .Select(p => new PostResponse(
                p.IdPost,
                p.IdComunidade,
                p.IdComunidadeNavigation != null ? p.IdComunidadeNavigation.Nome : null,
                p.IdUsuario,
                p.IdUsuarioNavigation != null ? p.IdUsuarioNavigation.Nome : null,
                p.Titulo,
                p.Conteudo,
                p.Imagem,
                p.Comentarios.Count,
                p.DataPublicacao
            ))
            .ToListAsync();

            return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPostPorId (int id)
    {
        var post = await _context.Posts
        .Include(p => p.IdUsuarioNavigation)
        .Include(p => p.IdComunidadeNavigation)
        .Include(p => p.Comentarios)
        .ThenInclude(c => c.IdUsuarioNavigation)
        .FirstOrDefaultAsync(p => p.IdPost == id);

        if(post == null)
        return NotFound(new {Message = "Post não encontrado."});

        var Comentarios = post.Comentarios
        .OrderBy(c => c.DataComentario)
        .Select(c => new ComentarioResponse(
            c.IdComentario,
            c.IdPost,
            c.IdUsuario,
            c.IdUsuarioNavigation != null ? c.IdUsuarioNavigation.Nome : null,
            c.Conteudo,
            c.DataComentario
        ))
        .ToList();

        return Ok(new{
        Post = new PostResponse(
            post.IdPost,
                post.IdComunidade,
                post.IdComunidadeNavigation?.Nome,
                post.IdUsuario,
                post.IdUsuarioNavigation?.Nome,
                post.Titulo,
                post.Conteudo,
                post.Imagem,
                post.Comentarios.Count,
                post.DataPublicacao
        ),
        Comentarios = comentarios
        });
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarPost([FromBody] CriarPostRequest request)
    {
        int usuarioId = ObterUsuarioIdLogado();

        if (request.IdComunidade.HasValue)
        {
            bool comunidadeExiste = await _context.Comunidades
                .AnyAsync(c => c.IdComunidade == request.IdComunidade.Value);

            if (!comunidadeExiste)
                return BadRequest(new { Message = "Comunidade informada não existe." });
        }

        var post = new Post
        {
            IdUsuario = usuarioId,
            IdComunidade = request.IdComunidade,
            Titulo = request.Titulo,
            Conteudo = request.Conteudo,
            Imagem = request.Imagem,
            DataPublicacao = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var response = new PostResponse(
            post.IdPost,
            post.IdComunidade,
            null,
            post.IdUsuario,
            User.Identity?.Name,
            post.Titulo,
            post.Conteudo,
            post.Imagem,
            0,
            post.DataPublicacao
        );

        return CreatedAtAction(nameof(ObterPostPorId), new { id = post.IdPost }, response);
    }

    [HttpPost("{id}/comentarios")]
    [Authorize]
    public async Task<IActionResult> AdicionarComentario(int id, [FromBody] CriarComentarioRequest request)
    {
        int usuarioId = ObterUsuarioIdLogado();

        bool postExiste = await _context.Posts.AnyAsync(p => p.IdPost == id);
        if (!postExiste)
            return NotFound(new { Message = "Post não encontrado." });

        var comentario = new Comentario
        {
            IdPost = id,
            IdUsuario = usuarioId,
            Conteudo = request.Conteudo,
            DataComentario = DateTime.UtcNow
        };

        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Comentário adicionado com sucesso.", IdComentario = comentario.IdComentario });
    }

    [HttpPost("{id}/comentarios")]
    [Authorize]
    public async Task<IActionResult> AdicionarComentario(int id, [FromBody] CriarComentarioRequest request)
    {
        int usuarioId = ObterUsuarioIdLogado();

        bool postExiste = await _context.Posts.AnyAsync(p => p.IdPost == id);
        if(!postExiste)
        return NotFound(new {Message = "Post não encontrado."});

        var comentario = new Comentario
        {
            IdPost = id,
            IdUsuario = usuarioId,
            Conteudo = request.Conteudo,
            DataComentario = DateTime.UtcNow
        };

        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();

        return Ok(new {Message = "Comentário adicionado com sucesso.", IdComentario = comentario.IdComentario});
    }
}


