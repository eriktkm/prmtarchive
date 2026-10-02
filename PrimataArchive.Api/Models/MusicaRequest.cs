using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record MusicaRequest(
    [Required(ErrorMessage = "O ID do artista é obrigatório.")]
    int IdArtista,

    int? IDAlbum,

    [Required(ErrorMessage = "O título da música é obrigatório.")]
    [StringLength(150, ErrorMessage = "O título não pode exceder 150 caracteres.")]
    string Titulo,

    int? Duracao,
    string? Genero,
    string? Capa,
    int? NumeroFaixa,
    DateOnly? DataLancamento
);