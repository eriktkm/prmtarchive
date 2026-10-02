using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record ArtistaRequest(
    [Required(ErrorMessage = "O nome do artista é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    string NomeArtista,
    string? Foto
);