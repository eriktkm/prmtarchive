using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PrimataArchive.Api.Models;

public partial class PrimataArchiveContext : DbContext
{
    public PrimataArchiveContext()
    {
    }

    public PrimataArchiveContext(DbContextOptions<PrimataArchiveContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artistum> Artista { get; set; }

    public virtual DbSet<Biblioteca> Bibliotecas { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Comunidade> Comunidades { get; set; }

    public virtual DbSet<ComunidadeMembro> ComunidadeMembros { get; set; }

    public virtual DbSet<Denuncium> Denuncia { get; set; }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Musica> Musicas { get; set; }

    public virtual DbSet<MusicaFavoritum> MusicaFavorita { get; set; }

    public virtual DbSet<Notificacao> Notificacaos { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<PlaylistMusica> PlaylistMusicas { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<SeguirArtistum> SeguirArtista { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.IdAlbum).HasName("PRIMARY");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Tipo).HasDefaultValueSql("'album'");

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Albums).HasConstraintName("fk_album_artista");
        });

        modelBuilder.Entity<Artistum>(entity =>
        {
            entity.HasKey(e => e.IdArtista).HasName("PRIMARY");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Artista).HasConstraintName("fk_artista_usuario");
        });

        modelBuilder.Entity<Biblioteca>(entity =>
        {
            entity.HasKey(e => e.IdBiblioteca).HasName("PRIMARY");

            entity.Property(e => e.DataAdicao).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdMusicaNavigation).WithMany(p => p.Bibliotecas).HasConstraintName("fk_biblioteca_musica");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Bibliotecas).HasConstraintName("fk_biblioteca_usuario");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdComentario).HasName("PRIMARY");

            entity.Property(e => e.DataComentario).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Comentarios).HasConstraintName("fk_comentario_post");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Comentarios).HasConstraintName("fk_comentario_usuario");
        });

        modelBuilder.Entity<Comunidade>(entity =>
        {
            entity.HasKey(e => e.IdComunidade).HasName("PRIMARY");

            entity.Property(e => e.DataCriacao).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdUsuarioCriadorNavigation).WithMany(p => p.Comunidades)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_comunidade_criador");
        });

        modelBuilder.Entity<ComunidadeMembro>(entity =>
        {
            entity.HasKey(e => new { e.IdComunidade, e.IdUsuario })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.Property(e => e.DataIngresso).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdComunidadeNavigation).WithMany(p => p.ComunidadeMembros).HasConstraintName("comunidade_membro_ibfk_1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ComunidadeMembros).HasConstraintName("comunidade_membro_ibfk_2");
        });

        modelBuilder.Entity<Denuncium>(entity =>
        {
            entity.HasKey(e => e.IdDenuncia).HasName("PRIMARY");

            entity.Property(e => e.DataDenuncia).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Status).HasDefaultValueSql("'pendente'");

            entity.HasOne(d => d.IdComentarioNavigation).WithMany(p => p.Denuncia)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_denuncia_comentario");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Denuncia)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_denuncia_post");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Denuncia).HasConstraintName("fk_denuncia_usuario");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.IdFavorito).HasName("PRIMARY");

            entity.Property(e => e.DataFavorito).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdAlbumNavigation).WithMany(p => p.Favoritos).HasConstraintName("fk_favorito_album");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Favoritos).HasConstraintName("fk_favorito_usuario");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero).HasName("PRIMARY");
        });

        modelBuilder.Entity<Musica>(entity =>
        {
            entity.HasKey(e => e.IdMusica).HasName("PRIMARY");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Reproducoes).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.IdAlbumNavigation).WithMany(p => p.Musicas)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_musica_album");

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Musicas).HasConstraintName("fk_musica_artista");
        });

        modelBuilder.Entity<MusicaFavoritum>(entity =>
        {
            entity.HasKey(e => e.IdMusicaFavorita).HasName("PRIMARY");

            entity.Property(e => e.DataFavorito).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdMusicaNavigation).WithMany(p => p.MusicaFavorita).HasConstraintName("fk_musica_favorita_musica");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MusicaFavorita).HasConstraintName("fk_musica_favorita_usuario");
        });

        modelBuilder.Entity<Notificacao>(entity =>
        {
            entity.HasKey(e => e.IdNotificacao).HasName("PRIMARY");

            entity.Property(e => e.DataNotificacao).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Lida).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Notificacaos).HasConstraintName("fk_notificacao_usuario");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.IdPlaylist).HasName("PRIMARY");

            entity.Property(e => e.DataCriacao).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Publica).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Playlists).HasConstraintName("fk_playlist_usuario");
        });

        modelBuilder.Entity<PlaylistMusica>(entity =>
        {
            entity.HasKey(e => new { e.IdPlaylist, e.IdMusica })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.Property(e => e.Ordem).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.IdMusicaNavigation).WithMany(p => p.PlaylistMusicas).HasConstraintName("fk_playlist_musica_musica");

            entity.HasOne(d => d.IdPlaylistNavigation).WithMany(p => p.PlaylistMusicas).HasConstraintName("fk_playlist_musica_playlist");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.IdPost).HasName("PRIMARY");

            entity.Property(e => e.DataPublicacao).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdComunidadeNavigation).WithMany(p => p.Posts)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_post_comunidade");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Posts).HasConstraintName("fk_post_usuario");
        });

        modelBuilder.Entity<SeguirArtistum>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdArtista })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.Property(e => e.DataSeguimento).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.SeguirArtista).HasConstraintName("fk_seguir_artista");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.SeguirArtista).HasConstraintName("fk_seguir_usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.TipoUsuario).HasDefaultValueSql("'usuario'");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
