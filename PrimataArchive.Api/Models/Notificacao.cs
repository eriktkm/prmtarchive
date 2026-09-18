using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("notificacao")]
[Index("IdUsuario", Name = "fk_notificacao_usuario")]
public partial class Notificacao
{
    [Key]
    [Column("id_notificacao")]
    public int IdNotificacao { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("tipo")]
    [StringLength(50)]
    public string Tipo { get; set; } = null!;

    [Column("mensagem")]
    [StringLength(255)]
    public string Mensagem { get; set; } = null!;

    [Column("lida")]
    public bool? Lida { get; set; }

    [Column("data_notificacao", TypeName = "datetime")]
    public DateTime? DataNotificacao { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Notificacaos")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
