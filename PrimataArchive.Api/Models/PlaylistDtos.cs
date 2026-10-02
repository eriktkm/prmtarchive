using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record CriarPlaylistRequest(
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100)] string Nome,
    string? Descricao,
    string? Capa,
    bool Publica = true
);

public record PlaylistResponse(
    int IdPLaylist,
    int IdUsuario,
    string? NomeUsuario,
    string Nome,
    string? Descricao,
    string? Capa,
    bool? Public,
    int TotalMusicas,
    DateTime? DataCriacao
);

public record AdicionarMusicaPlaylistRequest(
    [Required] int IdMusica
);