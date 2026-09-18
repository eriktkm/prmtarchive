using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("biblioteca")]
[Index("IdMusica", Name = "fk_biblioteca_musica")]
[Index("IdUsuario", "IdMusica", Name = "id_usuario", IsUnique = true)]
public partial class Biblioteca
{
    [Key]
    [Column("id_biblioteca")]
    public int IdBiblioteca { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_musica")]
    public int IdMusica { get; set; }

    [Column("data_adicao", TypeName = "datetime")]
    public DateTime? DataAdicao { get; set; }

    [ForeignKey("IdMusica")]
    [InverseProperty("Bibliotecas")]
    public virtual Musica IdMusicaNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("Bibliotecas")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
