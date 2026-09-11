# Primata Archive — integração Front-end / Spring Boot

O front-end foi preparado para trocar as ações demonstrativas por chamadas HTTP ao backend.

## 1. URL da API

Arquivo: `pages/api.js`

```js
var API_BASE_URL = "http://localhost:8080/api";
```

Se o Spring Boot usar outra porta ou contexto, altere somente essa variável.

## 2. Autenticação

O front envia:

`POST /api/auth/login`

Body:

```json
{
  "email": "usuario@email.com",
  "senha": "123456"
}
```

Resposta esperada:

```json
{
  "token": "JWT_AQUI",
  "usuario": {
    "id": 1,
    "username": "erik_archive",
    "email": "usuario@email.com"
  }
}
```

Cadastro:

`POST /api/auth/register`

```json
{
  "username": "erik_archive",
  "email": "usuario@email.com",
  "senha": "123456"
}
```

O token é salvo em `localStorage` somente para autenticar as chamadas do front. A senha não é salva.

## 3. Endpoints usados pelo front

### Usuário
- `PUT /api/usuarios/me`
- `PATCH /api/usuarios/me/preferencias`

### Comunidade
- `POST /api/comunidades`
- `POST /api/comunidades/{id}/entrar`
- `GET /api/comunidades/{id}`

### Publicação
- `POST /api/publicacoes`
- `POST /api/comentarios`

### Notificações
- `PATCH /api/notificacoes/marcar-todas-como-lidas`

### Lançamento
- `POST /api/lancamentos` usando `multipart/form-data`
- campos: `titulo`, `tipo`, `dataLancamento`, `genero`, `descricao`, `status`, `capa`, `audio`

### Música / conteúdo / administração
- `GET /api/usuarios/{id}`
- `GET /api/musicas/{id}`
- `PATCH /api/conteudos/{id}`
- `PATCH /api/denuncias/{id}`
- `GET /api/denuncias/conteudo/{id}`
- `GET /api/comunidades/{id}`

## 4. Listas públicas

Os containers principais já possuem `data-api-list` para facilitar a substituição do HTML estático por dados vindos do Spring Boot:

- `/musicas/destaques`
- `/musicas/top`
- `/playlists/publica`

## 5. CORS

Durante o desenvolvimento, o Spring Boot deverá permitir requisições do endereço onde o HTML estiver sendo executado, por exemplo:

`http://localhost:3000`

ou a origem usada pelo servidor estático.

## 6. Arquivos JavaScript

- `pages/api.js` — comunicação HTTP centralizada.
- `pages/auth.js` — login e cadastro.
- `pages/player.js` — player das páginas de música.
- `pages/social.js` — comunidades, posts, comentários, perfil e notificações.
- `pages/gestao.js` — configurações, lançamentos e telas administrativas.

Assim, quando o Spring Boot estiver pronto, o objetivo é alterar apenas os endpoints/nomes dos campos caso o contrato da API seja diferente.


## Biblioteca pessoal — novos endpoints esperados

As novas páginas pessoais não usam dados fictícios. Elas estão preparadas para o usuário autenticado:

- `GET /api/biblioteca/musicas` — músicas curtidas pelo usuário
- `GET /api/biblioteca/playlists` — playlists pessoais do usuário
- `GET /api/biblioteca/albuns` — álbuns/EPs curtidos pelo usuário
- `GET /api/busca?q=...` — pesquisa geral

Os nomes dos campos aceitos pelo front incluem `id`, `titulo`/`nome`, `artista`, `album`,
`duracao`, `capaUrl`, `audioUrl`, `tipo`, `descricao` e `quantidadeMusicas`.

Se o seu Spring Boot usar outros nomes de endpoints ou DTOs, basta ajustar esses caminhos no JS.
