using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record CriarDenunciaRequest(
    [Required(ErrorMessage = "O motivo da denúncia é obrigatório.")]
    string Motivo,
    string? Descricao,
    int? IdPost,
    int? IdComentario,
    int? IdUsuarioDenunciado
);

public record DenunciaResponse(
    int IdDenuncia,
    int IdUsuario,
    string Motivo,
    string? Descricao,
    string Status,
    DateTime? DataCriacao
);