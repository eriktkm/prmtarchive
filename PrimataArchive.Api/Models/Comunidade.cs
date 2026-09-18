using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("comunidade")]
[Index("IdUsuarioCriador", Name = "fk_comunidade_criador")]
[Index("Nome", Name = "nome", IsUnique = true)]
public partial class Comunidade
{
    [Key]
    [Column("id_comunidade")]
    public int IdComunidade { get; set; }

    [Column("id_usuario_criador")]
    public int? IdUsuarioCriador { get; set; }

    [Column("nome")]
    [StringLength(100)]
    public string Nome { get; set; } = null!;

    [Column("descricao", TypeName = "text")]
    public string? Descricao { get; set; }

    [Column("foto")]
    [StringLength(255)]
    public string? Foto { get; set; }

    [Column("data_criacao", TypeName = "datetime")]
    public DateTime? DataCriacao { get; set; }

    [InverseProperty("IdComunidadeNavigation")]
    public virtual ICollection<ComunidadeMembro> ComunidadeMembros { get; set; } = new List<ComunidadeMembro>();

    [ForeignKey("IdUsuarioCriador")]
    [InverseProperty("Comunidades")]
    public virtual Usuario? IdUsuarioCriadorNavigation { get; set; }

    [InverseProperty("IdComunidadeNavigation")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
