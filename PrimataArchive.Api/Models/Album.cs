using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("album")]
[Index("IdArtista", Name = "fk_album_artista")]
public partial class Album
{
    [Key]
    [Column("id_album")]
    public int IdAlbum { get; set; }

    [Column("id_artista")]
    public int IdArtista { get; set; }

    [Column("titulo")]
    [StringLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("capa")]
    [StringLength(255)]
    public string? Capa { get; set; }

    [Column("descricao", TypeName = "text")]
    public string? Descricao { get; set; }

    [Column("genero")]
    [StringLength(50)]
    public string? Genero { get; set; }

    [Column("data_lancamento")]
    public DateOnly? DataLancamento { get; set; }

    [Column("tipo", TypeName = "enum('album','ep','single')")]
    public string? Tipo { get; set; }

    [Column("data_cadastro", TypeName = "datetime")]
    public DateTime? DataCadastro { get; set; }

    [InverseProperty("IdAlbumNavigation")]
    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    [ForeignKey("IdArtista")]
    [InverseProperty("Albums")]
    public virtual Artistum IdArtistaNavigation { get; set; } = null!;

    [InverseProperty("IdAlbumNavigation")]
    public virtual ICollection<Musica> Musicas { get; set; } = new List<Musica>();
}
