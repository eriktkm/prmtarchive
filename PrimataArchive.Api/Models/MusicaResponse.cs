namespace PrimataArchive.Api.Models;

public record MusicaResponse(
    int IdMusica,
    int IdArtista,
    string? NomeArtista,
    int? IdAlbum,
    string? TituloAlbum,
    string Titulo,
    int? Duracao,
    string? Genero,
    string? Capa,
    int? NumeroFaixa,
    DateOnly? DataLancamento,
    int? Reproducoes,
    DateTime? DataCadastro
);