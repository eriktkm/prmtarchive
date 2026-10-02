using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record AdicionarFavoritoRequest(
    [Required(ErrorMessage = "O ID do áçbum é obrigatório.")]
    int IdAlbum
);

public record FavoritoResponse(
    int IdFavorito,
    int IdUsuario,
    int IdAlbum,
    string TituloAlbum,
    string? CapaAlbum,
    string? NomeArtista,
    DateTime? DataFavorito
);