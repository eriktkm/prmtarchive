namespace PrimataArchive.Api.Models;

public record AlbumResponse(
    int IdAlbum,
    int IdArtista,
    string? NomeArtista,
    string Titulo,
    string Capa,
    string? Descricao,
    string? Genero,
    DateOnly? DataLancamento,
    string? Tipo,
    DateTime? DataCadastro
);