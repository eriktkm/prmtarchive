using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrimataArchive.Api.Models;

[PrimaryKey("IdPlaylist", "IdMusica")]
[Table("playlist_musica")]
[Index("IdMusica", Name = "fk_playlist_musica_musica")]
public partial class PlaylistMusica
{
    [Key]
    [Column("id_playlist")]
    public int IdPlaylist { get; set; }

    [Key]
    [Column("id_musica")]
    public int IdMusica { get; set; }

    [Column("ordem")]
    public int? Ordem { get; set; }

    [ForeignKey("IdMusica")]
    [InverseProperty("PlaylistMusicas")]
    public virtual Musica IdMusicaNavigation { get; set; } = null!;

    [ForeignKey("IdPlaylist")]
    [InverseProperty("PlaylistMusicas")]
    public virtual Playlist IdPlaylistNavigation { get; set; } = null!;
}
