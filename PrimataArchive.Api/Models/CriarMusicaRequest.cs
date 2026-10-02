using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record CriarMusicaRequest(
    [Required] int IdArtista,
    int? IdAlbum,
    [Required] string Titulo,
    int? Duracao,
    string? Genero,
    string? Capa,
    int? NumeroFaixa,
    DateOnly? DataLancamento
);