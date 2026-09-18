using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("comentario")]
[Index("IdPost", Name = "fk_comentario_post")]
[Index("IdUsuario", Name = "fk_comentario_usuario")]
public partial class Comentario
{
    [Key]
    [Column("id_comentario")]
    public int IdComentario { get; set; }

    [Column("id_post")]
    public int IdPost { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("conteudo", TypeName = "text")]
    public string Conteudo { get; set; } = null!;

    [Column("data_comentario", TypeName = "datetime")]
    public DateTime? DataComentario { get; set; }

    [InverseProperty("IdComentarioNavigation")]
    public virtual ICollection<Denuncium> Denuncia { get; set; } = new List<Denuncium>();

    [ForeignKey("IdPost")]
    [InverseProperty("Comentarios")]
    public virtual Post IdPostNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("Comentarios")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
