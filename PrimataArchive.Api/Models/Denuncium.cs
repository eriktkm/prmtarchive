using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("denuncia")]
[Index("IdComentario", Name = "fk_denuncia_comentario")]
[Index("IdPost", Name = "fk_denuncia_post")]
[Index("IdUsuario", Name = "fk_denuncia_usuario")]
public partial class Denuncium
{
    [Key]
    [Column("id_denuncia")]
    public int IdDenuncia { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_post")]
    public int? IdPost { get; set; }

    [Column("id_comentario")]
    public int? IdComentario { get; set; }

    [Column("motivo")]
    [StringLength(255)]
    public string Motivo { get; set; } = null!;

    [Column("descricao", TypeName = "text")]
    public string? Descricao { get; set; }

    [Column("status", TypeName = "enum('pendente','analisando','resolvida','recusada')")]
    public string? Status { get; set; }

    [Column("data_denuncia", TypeName = "datetime")]
    public DateTime? DataDenuncia { get; set; }

    [ForeignKey("IdComentario")]
    [InverseProperty("Denuncia")]
    public virtual Comentario? IdComentarioNavigation { get; set; }

    [ForeignKey("IdPost")]
    [InverseProperty("Denuncia")]
    public virtual Post? IdPostNavigation { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Denuncia")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
