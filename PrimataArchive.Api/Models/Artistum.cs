using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("artista")]
[Index("EmailArtista", Name = "email_artista", IsUnique = true)]
[Index("IdUsuario", Name = "fk_artista_usuario")]
[Index("NomeArtista", Name = "nome_artista", IsUnique = true)]
public partial class Artistum
{
    [Key]
    [Column("id_artista")]
    public int IdArtista { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("nome_artista")]
    [StringLength(100)]
    public string NomeArtista { get; set; } = null!;

    [Column("email_artista")]
    [StringLength(150)]
    public string EmailArtista { get; set; } = null!;

    [Column("biografia", TypeName = "text")]
    public string Biografia { get; set; } = null!;

    [Column("foto")]
    [StringLength(255)]
    public string? Foto { get; set; }

    [Column("instagram")]
    [StringLength(255)]
    public string? Instagram { get; set; }

    [Column("spotify")]
    [StringLength(255)]
    public string? Spotify { get; set; }

    [Column("youtube")]
    [StringLength(255)]
    public string? Youtube { get; set; }

    [Column("soundcloud")]
    [StringLength(255)]
    public string? Soundcloud { get; set; }

    [Column("data_cadastro", TypeName = "datetime")]
    public DateTime? DataCadastro { get; set; }

    [InverseProperty("IdArtistaNavigation")]
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    [ForeignKey("IdUsuario")]
    [InverseProperty("Artista")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdArtistaNavigation")]
    public virtual ICollection<Musica> Musicas { get; set; } = new List<Musica>();

    [InverseProperty("IdArtistaNavigation")]
    public virtual ICollection<SeguirArtistum> SeguirArtista { get; set; } = new List<SeguirArtistum>();
}
