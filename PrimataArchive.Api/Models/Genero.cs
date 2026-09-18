using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("genero")]
[Index("Nome", Name = "nome", IsUnique = true)]
public partial class Genero
{
    [Key]
    [Column("id_genero")]
    public int IdGenero { get; set; }

    [Column("nome")]
    [StringLength(50)]
    public string Nome { get; set; } = null!;
}
