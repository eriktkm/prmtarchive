using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record CriarPostRquest(
    int? idComunidade,
    [StringLength(200)] string? Titulo,
    [Required(ErrorMessage = "O conteúdo do post é obrigatório.")] string? Titulo,
    string Conteudo,
    string? Imagem
);

public record PostResponse(
    int IdPost,
    int? IdComunidade,
    string? NomeComunidade,
    int IdUsuario,
    string? NomeUsuario,
    string? Titulo,
    string Conteudo,
    string? Imagem,
    int TotalComentarios,
    DateTime? DataPublicacao
);

public record CriarComentarioRequest(
    [Required(ErrorMessage = "O conteúdo do comentário é obrigatório.")]
    string Conteudo
);

public record ComentarioResponse(
    int IdComentario,
    int IdPost,
    int IdUsuario,
    string? NomeUsuario,
    string Conteudo,
    DateTime? DataComentario
);