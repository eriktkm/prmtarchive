namespace PrimataArchive.Api.Models;

public record ArtistaResponse(
    int IdArtista,
    string NomeArtista,
    string? Foto,
    DateTime? DataCadastro
);