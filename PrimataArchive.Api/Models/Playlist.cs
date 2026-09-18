using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[Table("playlist")]
[Index("IdUsuario", Name = "fk_playlist_usuario")]
public partial class Playlist
{
    [Key]
    [Column("id_playlist")]
    public int IdPlaylist { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("nome")]
    [StringLength(100)]
    public string Nome { get; set; } = null!;

    [Column("descricao", TypeName = "text")]
    public string? Descricao { get; set; }

    [Column("capa")]
    [StringLength(255)]
    public string? Capa { get; set; }

    [Column("publica")]
    public bool? Publica { get; set; }

    [Column("data_criacao", TypeName = "datetime")]
    public DateTime? DataCriacao { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Playlists")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdPlaylistNavigation")]
    public virtual ICollection<PlaylistMusica> PlaylistMusicas { get; set; } = new List<PlaylistMusica>();
}
