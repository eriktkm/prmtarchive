using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record CriarPostRequest(
    int? idComunidade,
    [StringLength(200)] string? Titulo,
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