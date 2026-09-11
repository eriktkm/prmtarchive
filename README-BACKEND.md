# Primata Archive — Frontend integrado ao Spring Boot

O frontend não possui catálogo de demonstração. Artistas, músicas, álbuns, playlists, comunidades, publicações, notificações, perfil e métricas são carregados da API.

## API base

Por padrão:

`http://localhost:8080/api`

Para trocar sem editar os arquivos:

```html
<script>window.PRIMATA_API_URL = "http://localhost:8080/api";</script>
```

## Endpoints esperados

### Autenticação
- `POST /auth/login`
- `POST /auth/register`

### Catálogo
- `GET /artistas`
- `GET /artistas/destaques`
- `GET /artistas/{id}`
- `GET /artistas/{id}/musicas`
- `POST /artistas/{id}/seguir`
- `GET /musicas/top`
- `GET /musicas/destaques`
- `GET /musicas/{id}`
- `POST /musicas/{id}/curtir`
- `GET /albuns`
- `GET /playlists/publica`
- `GET /playlists/destaque`

### Biblioteca autenticada
- `GET /biblioteca/resumo`
- `GET /biblioteca/artistas`
- `GET /biblioteca/musicas`
- `GET /biblioteca/musicas/recentes`
- `GET /biblioteca/albuns`
- `GET /biblioteca/playlists`
- `GET /playlists/minhas`

### Comunidade
- `GET /comunidades`
- `GET /comunidades/{id}`
- `POST /comunidades/{id}/entrar`
- `GET /comunidades/{id}/publicacoes`
- `GET /publicacoes/recentes`
- `GET /publicacoes/{id}`
- `GET /publicacoes/{id}/comentarios`
- `POST /comentarios`

### Conta
- `GET /usuarios/me`
- `GET /usuarios/me/atividade`
- `PUT /usuarios/me`
- `GET /notificacoes`
- `PATCH /notificacoes/marcar-todas-como-lidas`

### Administração / artista
As telas administrativas usam endpoints separados para não misturar dados de demonstração com dados reais:
- `GET /artista/musicas`
- `GET /artista/dashboard`
- `GET /artista/estatisticas`
- `GET /admin/usuarios`
- `GET /admin/conteudos`
- `GET /admin/denuncias`
- `GET /admin/comunidades`
- `GET /admin/dashboard`

## Regras do frontend

- Nenhuma música, artista, álbum, playlist ou publicação é criada como dado fictício no HTML/JS.
- As listas são preenchidas dinamicamente com os objetos retornados pela API.
- Os links de detalhes usam o `id` recebido do backend.
- Se a API estiver indisponível, a tela mostra o estado de backend indisponível em vez de substituir por conteúdo falso.
- O token JWT é enviado no header `Authorization: Bearer ...`.
