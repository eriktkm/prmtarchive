using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[PrimaryKey("IdComunidade", "IdUsuario")]
[Table("comunidade_membro")]
[Index("IdUsuario", Name = "id_usuario")]
public partial class ComunidadeMembro
{
    [Key]
    [Column("id_comunidade")]
    public int IdComunidade { get; set; }

    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("data_ingresso", TypeName = "datetime")]
    public DateTime? DataIngresso { get; set; }

    [ForeignKey("IdComunidade")]
    [InverseProperty("ComunidadeMembros")]
    public virtual Comunidade IdComunidadeNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("ComunidadeMembros")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
