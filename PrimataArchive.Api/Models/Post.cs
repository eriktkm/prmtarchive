using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("post")]
[Index("IdComunidade", Name = "fk_post_comunidade")]
[Index("IdUsuario", Name = "fk_post_usuario")]
public partial class Post
{
    [Key]
    [Column("id_post")]
    public int IdPost { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_comunidade")]
    public int? IdComunidade { get; set; }

    [Column("titulo")]
    [StringLength(200)]
    public string? Titulo { get; set; }

    [Column("conteudo", TypeName = "text")]
    public string Conteudo { get; set; } = null!;

    [Column("imagem")]
    [StringLength(255)]
    public string? Imagem { get; set; }

    [Column("data_publicacao", TypeName = "datetime")]
    public DateTime? DataPublicacao { get; set; }

    [InverseProperty("IdPostNavigation")]
    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    [InverseProperty("IdPostNavigation")]
    public virtual ICollection<Denuncium> Denuncia { get; set; } = new List<Denuncium>();

    [ForeignKey("IdComunidade")]
    [InverseProperty("Posts")]
    public virtual Comunidade? IdComunidadeNavigation { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Posts")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
