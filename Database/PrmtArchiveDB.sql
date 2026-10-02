CREATE DATABASE IF NOT EXISTS primata_archive;
USE primata_archive;

CREATE TABLE usuario (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL UNIQUE,
    email VARCHAR(150) NOT NULL UNIQUE,
    foto VARCHAR(255) NULL,
    senha VARCHAR(255) NOT NULL,
    tipo_usuario ENUM('usuario', 'artista', 'admin') DEFAULT 'usuario',
    data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP,
    bio VARCHAR(155),
    notificacoes BOOLEAN DEFAULT TRUE
);

CREATE TABLE artista (
    id_artista INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    email_artista VARCHAR(50) NOT NULL UNIQUE,
    nome_artistico VARCHAR(100) NOT NULL,
    biografia TEXT,
    foto VARCHAR(255),
    instagram VARCHAR(255),
    spotify VARCHAR(255),
    youtube VARCHAR(255),
    soundcloud VARCHAR(255), 
    data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_artista_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

CREATE TABLE genero (
    id_genero INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE album (
    id_album INT AUTO_INCREMENT PRIMARY KEY,
    id_artista INT NOT NULL,
    id_genero INT NULL,
    titulo VARCHAR(150) NOT NULL,
    capa VARCHAR(255),
    descricao TEXT,
    data_lancamento DATE,
    tipo ENUM('album', 'ep', 'single') DEFAULT 'album',
    data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_album_artista
        FOREIGN KEY (id_artista)
        REFERENCES artista(id_artista)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT fk_album_genero
        FOREIGN KEY (id_genero)
        REFERENCES genero(id_genero)
        ON DELETE SET NULL
);

CREATE TABLE musica (
    id_musica INT AUTO_INCREMENT PRIMARY KEY,
    id_album INT,
    id_artista INT NOT NULL,
    titulo VARCHAR(150) NOT NULL,
    duracao INT,
    capa VARCHAR(255),
    numero_faixa INT,
    data_lancamento DATE,
    reproducoes INT DEFAULT 0,
    data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_musica_album
        FOREIGN KEY (id_album)
        REFERENCES album(id_album)
        ON DELETE SET NULL
        ON UPDATE CASCADE,

    CONSTRAINT fk_musica_artista
        FOREIGN KEY (id_artista)
        REFERENCES artista(id_artista)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

CREATE TABLE musica_favorita (
    id_usuario INT NOT NULL,
    id_musica INT NOT NULL,
    data_favorito DATETIME DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY (id_usuario, id_musica),

    CONSTRAINT fk_musica_favorita_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE,

    CONSTRAINT fk_musica_favorita_musica
        FOREIGN KEY (id_musica)
        REFERENCES musica(id_musica)
        ON DELETE CASCADE
);

CREATE TABLE biblioteca (
    id_usuario INT NOT NULL,
    id_musica INT NOT NULL,
    data_adicao DATETIME DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY (id_usuario, id_musica),

    CONSTRAINT fk_biblioteca_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE,

    CONSTRAINT fk_biblioteca_musica
        FOREIGN KEY (id_musica)
        REFERENCES musica(id_musica)
        ON DELETE CASCADE
);

CREATE TABLE playlist (
    id_playlist INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    nome VARCHAR(100) NOT NULL,
    descricao TEXT,
    capa VARCHAR(255),
    publica BOOLEAN DEFAULT FALSE,
    data_criacao DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_playlist_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE
);

CREATE TABLE playlist_musica (
    id_playlist INT NOT NULL,
    id_musica INT NOT NULL,
    ordem INT DEFAULT 0,

    PRIMARY KEY (id_playlist, id_musica),

    CONSTRAINT fk_playlist_musica_playlist
        FOREIGN KEY (id_playlist)
        REFERENCES playlist(id_playlist)
        ON DELETE CASCADE,

    CONSTRAINT fk_playlist_musica_musica
        FOREIGN KEY (id_musica)
        REFERENCES musica(id_musica)
        ON DELETE CASCADE
);

CREATE TABLE favorito (
    id_usuario INT NOT NULL,
    id_album INT NOT NULL,
    data_favorito DATETIME DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY (id_usuario, id_album),

    CONSTRAINT fk_favorito_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT fk_favorito_album
        FOREIGN KEY (id_album)
        REFERENCES album(id_album)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

CREATE TABLE avaliacao (
    id_avaliacao INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_album INT NOT NULL,
    nota INT NOT NULL,
    comentario TEXT,
    data_avaliacao DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_avaliacao_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT fk_avaliacao_album
        FOREIGN KEY (id_album)
        REFERENCES album(id_album)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT chk_nota CHECK (nota >= 1 AND nota <= 5),
    CONSTRAINT uk_avaliacao_usuario_album UNIQUE (id_usuario, id_album) 
);

CREATE TABLE comunidade (
    id_comunidade INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario_criador INT NULL,
    nome VARCHAR(100) NOT NULL UNIQUE,
    descricao TEXT,
    foto VARCHAR(255),
    data_criacao DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_comunidade_criador
        FOREIGN KEY (id_usuario_criador)
        REFERENCES usuario(id_usuario)
        ON DELETE SET NULL
);

CREATE TABLE comunidade_membro (
    id_comunidade INT NOT NULL,
    id_usuario INT NOT NULL,
    data_ingresso DATETIME DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY (id_comunidade, id_usuario),

    FOREIGN KEY (id_comunidade) REFERENCES comunidade(id_comunidade) ON DELETE CASCADE,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario) ON DELETE CASCADE
);

CREATE TABLE post (
    id_post INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_comunidade INT,
    titulo VARCHAR(200),
    conteudo TEXT NOT NULL,
    imagem VARCHAR(255),
    data_publicacao DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_post_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE,

    CONSTRAINT fk_post_comunidade
        FOREIGN KEY (id_comunidade)
        REFERENCES comunidade(id_comunidade)
        ON DELETE CASCADE
);

CREATE TABLE comentario (
    id_comentario INT AUTO_INCREMENT PRIMARY KEY,
    id_post INT NOT NULL,
    id_usuario INT NOT NULL,
    conteudo TEXT NOT NULL,
    data_comentario DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_comentario_post
        FOREIGN KEY (id_post)
        REFERENCES post(id_post)
        ON DELETE CASCADE,

    CONSTRAINT fk_comentario_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE
);

CREATE TABLE denuncia (
    id_denuncia INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_post INT,
    id_comentario INT,
    motivo VARCHAR(255) NOT NULL,
    descricao TEXT,
    status ENUM('pendente', 'analisando', 'resolvida', 'recusada') DEFAULT 'pendente',
    data_denuncia DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_denuncia_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE,

    CONSTRAINT fk_denuncia_post
        FOREIGN KEY (id_post)
        REFERENCES post(id_post)
        ON DELETE CASCADE,

    CONSTRAINT fk_denuncia_comentario
        FOREIGN KEY (id_comentario)
        REFERENCES comentario(id_comentario)
        ON DELETE CASCADE,

    CONSTRAINT chk_alvo_denuncia CHECK (
        (id_post IS NOT NULL AND id_comentario IS NULL) OR 
        (id_post IS NULL AND id_comentario IS NOT NULL)
    )
);

CREATE TABLE notificacao (
    id_notificacao INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    tipo VARCHAR(50) NOT NULL,
    mensagem VARCHAR(255) NOT NULL,
    lida BOOLEAN DEFAULT FALSE,
    data_notificacao DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_notificacao_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE
);

CREATE TABLE seguir_artista (
    id_usuario INT NOT NULL,
    id_artista INT NOT NULL,
    data_seguimento DATETIME DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY (id_usuario, id_artista),

    CONSTRAINT fk_seguir_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario)
        ON DELETE CASCADE,

    CONSTRAINT fk_seguir_artista
        FOREIGN KEY (id_artista)
        REFERENCES artista(id_artista)
        ON DELETE CASCADE
);

INSERT INTO genero (nome) VALUES
('Hip-Hop'), ('Trap'), ('Boom Bap'), ('Cloud Rap'), ('Drill'), ('Emo Rap'), ('Phonk'),
('Rock'), ('Alternative Rock'), ('Indie Rock'), ('Post-Rock'), ('Punk Rock'), ('Hard Rock'),
('Metal'), ('Death Metal'), ('Black Metal'), ('Thrash Metal'), ('Doom Metal'), ('Nu Metal'),
('Metalcore'), ('Eletrônica'), ('Ambient'), ('Acid'), ('Techno'), ('House'), ('Deep House'),
('Industrial'), ('IDM'), ('Drum and Bass'), ('Breakcore'), ('Dubstep'), ('Trance'), ('Pop'),
('Synthpop'), ('Art Pop'), ('Hyperpop'), ('R&B'), ('Soul'), ('Neo Soul'), ('Indie'),
('Experimental'), ('Avant-Garde'), ('Noise'), ('Lo-Fi'), ('Jazz'), ('Fusion');

INSERT INTO usuario(nome, email, senha, tipo_usuario)
VALUES('Administrador', 'admin@primataarchive.com', '123456', 'admin');
SELECT  * FROM usuario;