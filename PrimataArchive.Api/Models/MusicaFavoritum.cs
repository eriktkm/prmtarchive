using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("musica_favorita")]
[Index("IdMusica", Name = "fk_musica_favorita_musica")]
[Index("IdUsuario", "IdMusica", Name = "id_usuario", IsUnique = true)]
public partial class MusicaFavoritum
{
    [Key]
    [Column("id_musica_favorita")]
    public int IdMusicaFavorita { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_musica")]
    public int IdMusica { get; set; }

    [Column("data_favorito", TypeName = "datetime")]
    public DateTime? DataFavorito { get; set; }

    [ForeignKey("IdMusica")]
    [InverseProperty("MusicaFavorita")]
    public virtual Musica IdMusicaNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("MusicaFavorita")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
