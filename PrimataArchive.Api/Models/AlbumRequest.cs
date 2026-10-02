using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record AlbumRequest(
    [Required(ErrorMessage = "O ID do artista é obrigatório.")]
    int IdArtista,

    [Required(ErrorMessage = "O título do álbum é obrigatório.")]
    [StringLength(150, ErrorMessage = "O título não pode exceder 150 caracteres.")]
    string Titulo,

    string? Descricao,
    string? Genero,
    string? Capa,
    DateOnly? DataLancamento,
    string? Tipo
);