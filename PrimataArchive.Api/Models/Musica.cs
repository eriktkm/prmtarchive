using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("musica")]
[Index("IdAlbum", Name = "fk_musica_album")]
[Index("IdArtista", Name = "fk_musica_artista")]
public partial class Musica
{
    [Key]
    [Column("id_musica")]
    public int IdMusica { get; set; }

    [Column("id_album")]
    public int? IdAlbum { get; set; }

    [Column("id_artista")]
    public int IdArtista { get; set; }

    [Column("titulo")]
    [StringLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("duracao")]
    public int? Duracao { get; set; }

    [Column("genero")]
    [StringLength(50)]
    public string? Genero { get; set; }

    [Column("capa")]
    [StringLength(255)]
    public string? Capa { get; set; }

    [Column("numero_faixa")]
    public int? NumeroFaixa { get; set; }

    [Column("data_lancamento")]
    public DateOnly? DataLancamento { get; set; }

    [Column("reproducoes")]
    public int? Reproducoes { get; set; }

    [Column("data_cadastro", TypeName = "datetime")]
    public DateTime? DataCadastro { get; set; }

    [InverseProperty("IdMusicaNavigation")]
    public virtual ICollection<Biblioteca> Bibliotecas { get; set; } = new List<Biblioteca>();

    [ForeignKey("IdAlbum")]
    [InverseProperty("Musicas")]
    public virtual Album? IdAlbumNavigation { get; set; }

    [ForeignKey("IdArtista")]
    [InverseProperty("Musicas")]
    public virtual Artistum IdArtistaNavigation { get; set; } = null!;

    [InverseProperty("IdMusicaNavigation")]
    public virtual ICollection<MusicaFavoritum> MusicaFavorita { get; set; } = new List<MusicaFavoritum>();

    [InverseProperty("IdMusicaNavigation")]
    public virtual ICollection<PlaylistMusica> PlaylistMusicas { get; set; } = new List<PlaylistMusica>();
}
