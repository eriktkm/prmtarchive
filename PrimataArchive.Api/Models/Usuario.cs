using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("usuario")]
[Index("Email", Name = "email", IsUnique = true)]
[Index("Nome", Name = "nome", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("nome")]
    [StringLength(100)]
    public string Nome { get; set; } = null!;

    [Column("email")]
    [StringLength(150)]
    public string Email { get; set; } = null!;

    [Column("foto")]
    [StringLength(255)]
    public string? Foto { get; set; }

    [Column("senha")]
    [StringLength(255)]
    public string Senha { get; set; } = null!;

    [Column("tipo_usuario", TypeName = "enum('usuario','artista','admin')")]
    public string? TipoUsuario { get; set; }

    [Column("data_cadastro", TypeName = "datetime")]
    public DateTime? DataCadastro { get; set; }

    [Column("bio")]
    [StringLength(155)]
    public string? Bio { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Artistum> Artista { get; set; } = new List<Artistum>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Biblioteca> Bibliotecas { get; set; } = new List<Biblioteca>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<ComunidadeMembro> ComunidadeMembros { get; set; } = new List<ComunidadeMembro>();

    [InverseProperty("IdUsuarioCriadorNavigation")]
    public virtual ICollection<Comunidade> Comunidades { get; set; } = new List<Comunidade>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Denuncium> Denuncia { get; set; } = new List<Denuncium>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<MusicaFavoritum> MusicaFavorita { get; set; } = new List<MusicaFavoritum>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Notificacao> Notificacaos { get; set; } = new List<Notificacao>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<SeguirArtistum> SeguirArtista { get; set; } = new List<SeguirArtistum>();
}
