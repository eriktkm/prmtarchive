using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[PrimaryKey("IdUsuario", "IdArtista")]
[Table("seguir_artista")]
[Index("IdArtista", Name = "fk_seguir_artista")]
public partial class SeguirArtistum
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Key]
    [Column("id_artista")]
    public int IdArtista { get; set; }

    [Column("data_seguimento", TypeName = "datetime")]
    public DateTime? DataSeguimento { get; set; }

    [ForeignKey("IdArtista")]
    [InverseProperty("SeguirArtista")]
    public virtual Artistum IdArtistaNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("SeguirArtista")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
