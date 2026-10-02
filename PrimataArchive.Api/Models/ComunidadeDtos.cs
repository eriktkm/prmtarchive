using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record CriarComunidadeRequest
(
    [Required(ErrorMessage = "O nome da comunidade é obrigatório.")]
    [StringLength(100)] string Nome,
    string? Descricao,
    string? Foto
);

public record ComunidadeResponse
(
    int IdComunidade,
    int? IdUsuarioCriador,
    string? NomeCriador,
    string Nome,
    string? Descricao,
    string? Foto,
    int TotalMembros,
    DateTime? DataCriacao
);