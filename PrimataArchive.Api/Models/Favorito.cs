using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("favorito")]
[Index("IdAlbum", Name = "fk_favorito_album")]
[Index("IdUsuario", "IdAlbum", Name = "uk_favorito_usuario_album", IsUnique = true)]
public partial class Favorito
{
    [Key]
    [Column("id_favorito")]
    public int IdFavorito { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_album")]
    public int IdAlbum { get; set; }

    [Column("data_favorito", TypeName = "datetime")]
    public DateTime? DataFavorito { get; set; }

    [ForeignKey("IdAlbum")]
    [InverseProperty("Favoritos")]
    public virtual Album IdAlbumNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("Favoritos")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
