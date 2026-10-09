namespace PrimataArchive.Api.Models;

public record AdicionarBibliotecaRequest(
    int? IdAlbum,
    int? IdPlaylist
);

public record BibliotecaResponse(
    int IdBiblioteca,
    int IdUsuario,
    int? IdAlbum,
    string? TituloAlbum,
    int? IdPlaylist,
    string? NomePlaylist,
    DateTime? DataAdicao
);