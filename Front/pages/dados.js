/* PRIMATA ARCHIVE - RENDERIZAÇÃO DINÂMICA DO BACKEND
   Nenhum conteúdo musical/artístico é criado no frontend.
   Todas as listas, detalhes e contadores vêm da API.
*/
(function () {
    "use strict";

    function arr(data, keys) {
        if (Array.isArray(data)) return data;
        for (var i = 0; i < keys.length; i++) {
            if (Array.isArray(data && data[keys[i]])) return data[keys[i]];
        }
        return [];
    }

    function text(value, fallback) {
        return value === null || value === undefined || value === "" ? (fallback || "") : String(value);
    }

    function esc(value) {
        return text(value).replace(/[&<>\"']/g, function (c) {
            return ({"&":"&amp;","<":"&lt;",">":"&gt;","\"":"&quot;","'":"&#039;"})[c];
        });
    }

    function image(url, alt, className) {
        return url ? '<img class="' + (className || "") + '" src="' + esc(url) + '" alt="' + esc(alt || "") + '">' : '';
    }

    function idOf(item) {
        return item && (item.id !== undefined ? item.id : item.idArtista !== undefined ? item.idArtista : item.idMusica);
    }

    function cover(item, type) {
        var url = item && (item.capaUrl || item.coverUrl || item.imagemUrl || item.fotoUrl || item.avatarUrl);
        return '<div class="dynamic-cover">' + image(url, text(item && (item.nome || item.titulo || type), type), "") + '</div>';
    }

    function state(container, title, message) {
        if (!container) return;
        container.innerHTML = '<div class="backend-state"><strong>' + esc(title) + '</strong><span>' + esc(message || "") + '</span></div>';
    }

    async function get(endpoint, container) {
        try {
            var data = await PrimataAPI.get(endpoint);
            return data;
        } catch (error) {
            state(container, "BACKEND NÃO DISPONÍVEL", error.message || "Não foi possível carregar os dados.");
            return null;
        }
    }

    function renderArtists(container, items) {
        container.innerHTML = "";
        if (!items.length) { state(container, "NENHUM ARTISTA", "O backend ainda não retornou artistas."); return; }
        items.forEach(function (item, index) {
            var id = idOf(item);
            var a = document.createElement("a");
            a.className = "artist-card dynamic-item";
            a.href = "../perfil-artista/index.html?id=" + encodeURIComponent(id);
            a.innerHTML = cover(item, "ARTISTA") +
                '<span>' + String(index + 1).padStart(2, "0") + '</span>' +
                '<h3>' + esc(item.nome || item.nomeArtistico || item.username || "Artista") + '</h3>' +
                '<p>' + esc(item.genero || item.generoMusical || "") + '</p>';
            container.appendChild(a);
        });
    }

    function renderTracks(container, items) {
        container.innerHTML = "";
        if (!items.length) { state(container, "NENHUMA MÚSICA", "O backend ainda não retornou músicas."); return; }
        items.forEach(function (item, index) {
            var id = idOf(item);
            var el = document.createElement("a");
            el.className = "track track-link dynamic-item";
            el.href = "../musica/index.html?id=" + encodeURIComponent(id);
            el.setAttribute("data-audio-url", item.audioUrl || item.urlAudio || "");
            el.innerHTML = '<span class="track-number">' + String(index + 1).padStart(2, "0") + '</span>' +
                cover(item, "MÚSICA") +
                '<div class="track-data"><strong>' + esc(item.titulo || item.nome || "Música") + '</strong><span>' + esc(item.artista || item.artistaNome || item.nomeArtista || "") + '</span></div>' +
                '<span class="track-album">' + esc(item.album || item.albumNome || "") + '</span>' +
                '<span class="track-time">' + esc(item.duracao || "") + '</span>' +
                '<span class="track-play">▶</span>';
            container.appendChild(el);
        });
        if (window.PrimataPlayer) window.PrimataPlayer.refresh();
    }

    function renderAlbums(container, items) {
        container.innerHTML = "";
        if (!items.length) { state(container, "NENHUM ÁLBUM / EP", "O backend ainda não retornou lançamentos."); return; }
        items.forEach(function (item) {
            var id = idOf(item);
            var a = document.createElement("a");
            a.className = "card release-card dynamic-item";
            a.href = "../albuns/index.html?id=" + encodeURIComponent(id);
            a.innerHTML = cover(item, "ÁLBUM") + '<strong>' + esc(item.titulo || item.nome || "Lançamento") + '</strong><span>' + esc(item.artista || item.artistaNome || "") + (item.tipo ? " • " + esc(item.tipo) : "") + '</span>';
            container.appendChild(a);
        });
    }

    function renderPlaylists(container, items) {
        container.innerHTML = "";
        if (!items.length) { state(container, "NENHUMA PLAYLIST", "O backend ainda não retornou playlists."); return; }
        items.forEach(function (item) {
            var id = idOf(item);
            var a = document.createElement("a");
            a.className = "card dynamic-item";
            a.href = item.url || "../playlist/index.html?id=" + encodeURIComponent(id);
            a.innerHTML = cover(item, "PLAYLIST") + '<strong>' + esc(item.nome || item.titulo || "Playlist") + '</strong><span>' + esc(item.descricao || "") + '</span>';
            container.appendChild(a);
        });
    }

    async function initListPage() {
        var nodes = document.querySelectorAll("[data-backend-list]");
        for (var i = 0; i < nodes.length; i++) {
            var node = nodes[i];
            var endpoint = node.getAttribute("data-backend-list");
            var data = await get(endpoint, node);
            if (data === null) continue;
            var items = arr(data, ["items", "content", "data", "artistas", "musicas", "albuns", "eps", "playlists", "publicacoes"]);
            var type = node.getAttribute("data-list-type");
            if (type === "artists") renderArtists(node, items);
            else if (type === "tracks") renderTracks(node, items);
            else if (type === "albums") renderAlbums(node, items);
            else if (type === "playlists") renderPlaylists(node, items);
            else if (type === "communities") renderCommunities(node, items);
            else if (type === "reports") renderReports(node, items);
        }
    }

    async function initArtistDetail() {
        var root = document.querySelector("[data-artist-detail]");
        if (!root) return;
        var id = new URLSearchParams(location.search).get("id");
        if (!id) { state(root, "ARTISTA NÃO INFORMADO", "Abra o perfil a partir de um artista retornado pelo backend."); return; }
        var data = await get("/artistas/" + encodeURIComponent(id), root);
        if (!data) return;
        root.innerHTML = '<div class="detail-cover">' + cover(data, "ARTISTA") + '</div>' +
            '<div class="detail-info"><span class="mini-title">ARTISTA</span><h1>' + esc(data.nome || data.nomeArtistico || "Artista") + '</h1>' +
            '<p>' + esc(data.bio || data.biografia || "") + '</p><div class="detail-meta">' + esc(data.genero || data.generoMusical || "") + (data.localizacao ? " • " + esc(data.localizacao) : "") + '</div>' +
            '<button class="pixel-button" data-follow-artist="' + esc(id) + '">SEGUIR</button></div>';
        root.querySelector("[data-follow-artist]").addEventListener("click", async function () {
            try { await PrimataAPI.post("/artistas/" + encodeURIComponent(id) + "/seguir"); this.textContent = "SEGUINDO"; this.disabled = true; } catch (e) { mostrarErroAPI(e); }
        });
        var tracks = document.querySelector("[data-artist-tracks]");
        if (tracks) { var list = await get("/artistas/" + encodeURIComponent(id) + "/musicas", tracks); if (list) renderTracks(tracks, arr(list, ["musicas", "items", "content", "data"])); }
    }

    async function initMusicDetail() {
        var root = document.querySelector("[data-music-detail]");
        if (!root) return;
        var id = new URLSearchParams(location.search).get("id");
        if (!id) { state(root, "MÚSICA NÃO INFORMADA", "Abra uma música retornada pelo backend."); return; }
        var data = await get("/musicas/" + encodeURIComponent(id), root);
        if (!data) return;
        root.innerHTML = '<div class="detail-cover">' + cover(data, "MÚSICA") + '</div><div class="detail-info"><span class="mini-title">MÚSICA</span><h1>' + esc(data.titulo || data.nome || "Música") + '</h1><p>' + esc(data.descricao || "") + '</p><div class="detail-meta">' + esc(data.artista || data.artistaNome || "") + (data.album ? " • " + esc(data.album) : "") + (data.duracao ? " • " + esc(data.duracao) : "") + '</div><button class="pixel-button" data-like-music="' + esc(id) + '">CURTIR</button><button class="pixel-button alt" data-play-detail>▶ OUVIR</button></div>';
        root.querySelector("[data-like-music]").addEventListener("click", async function () { try { await PrimataAPI.post("/musicas/" + encodeURIComponent(id) + "/curtir"); this.textContent = "CURTIDA"; this.disabled = true; } catch (e) { mostrarErroAPI(e); } });
        root.querySelector("[data-play-detail]").addEventListener("click", function () { if (window.PrimataPlayer) window.PrimataPlayer.play(data); });
    }


    async function initGenericTables() {
        var tables = document.querySelectorAll("[data-backend-table]");
        for (var i = 0; i < tables.length; i++) {
            var table = tables[i];
            var data = await get(table.getAttribute("data-backend-table"), table.querySelector("tbody") || table);
            if (!data) continue;
            var items = arr(data, ["items", "content", "data", "musicas", "usuarios", "conteudos", "denuncias", "comunidades"]);
            var tbody = table.querySelector("tbody");
            if (!tbody) continue;
            tbody.innerHTML = "";
            if (!items.length) { tbody.innerHTML = '<tr><td colspan="10"><div class="backend-state"><strong>NENHUM REGISTRO</strong><span>O backend não retornou dados.</span></div></td></tr>'; continue; }
            items.forEach(function (item) {
                var tr = document.createElement("tr");
                tr.dataset.id = idOf(item) || "";
                tr.innerHTML = '<td><strong>' + esc(item.titulo || item.nome || item.username || item.conteudo || "") + '</strong><br><span class="muted">' + esc(item.email || item.duracao || item.descricao || "") + '</span></td>' +
                    '<td>' + esc(item.album || item.tipo || item.perfil || item.autor || "") + '</td>' +
                    '<td>' + esc(item.plays ?? item.reproducoes ?? item.dataCadastro ?? "") + '</td>' +
                    '<td><span class="status">' + esc(item.status || "") + '</span></td>' +
                    '<td><button type="button" class="pixel-button alt" data-record-id="' + esc(idOf(item)) + '">VER</button></td>';
                tbody.appendChild(tr);
            });
        }
    }

    async function initCommunities() {
        var detail = document.querySelector("[data-community-detail]");
        if (!detail) return;
        var id = new URLSearchParams(location.search).get("id");
        if (!id) { state(detail, "COMUNIDADE NÃO INFORMADA", "Abra uma comunidade retornada pelo backend."); return; }
        var data = await get("/comunidades/" + encodeURIComponent(id), detail);
        if (data) {
            detail.innerHTML = '<div><span class="mini-title">COMMUNITY</span><h1>' + esc(data.nome || "Comunidade") + '</h1><p>' + esc(data.descricao || "") + '</p><div class="community-intro"><span class="badge">' + esc(data.categoria || "") + '</span><span class="meta">' + esc(data.quantidadeMembros ?? data.membrosCount ?? "") + '</span><button class="pixel-button" data-join-community>ENTRAR NA COMUNIDADE</button></div></div>';
            var join = detail.querySelector("[data-join-community]");
            if (join) join.addEventListener("click", async function () { try { await PrimataAPI.post("/comunidades/" + encodeURIComponent(id) + "/entrar"); join.textContent="MEMBRO"; join.disabled=true; } catch(e){ mostrarErroAPI(e); } });
            var posts = document.querySelector("[data-community-posts]");
            if (posts) { var pd = await get("/comunidades/" + encodeURIComponent(id) + "/publicacoes", posts); if (pd) renderPosts(posts, arr(pd,["publicacoes","posts","items","content","data"])); }
        }
    }

    function renderCommunities(container, items) {
        container.innerHTML="";
        if(!items.length){state(container,"NENHUMA COMUNIDADE","O backend não retornou comunidades.");return;}
        items.forEach(function(item){var a=document.createElement("a");a.className="card dynamic-item";a.href="../comunidade/index.html";a.innerHTML='<div class="spread"><strong>'+esc(item.nome||"")+'</strong><span class="status">'+esc(item.status||"")+'</span></div><p class="meta">'+esc(item.descricao||"")+'</p>';container.appendChild(a);});
    }
    function renderReports(container, items) {
        container.innerHTML="";
        if(!items.length){state(container,"NENHUMA DENÚNCIA","O backend não retornou denúncias.");return;}
        items.forEach(function(item){var box=document.createElement("div");box.className="pixel-card dynamic-item";box.dataset.id=idOf(item)||"";box.innerHTML='<div class="spread"><div><span class="code-label">'+esc(item.codigo||item.id||"")+'</span><h3>'+esc(item.titulo||item.motivo||item.tipo||"DENÚNCIA")+'</h3><p>'+esc(item.descricao||item.conteudo||"")+'</p></div><span class="status">'+esc(item.status||"")+'</span></div><div class="actions"><button class="pixel-button alt" data-report-action="analisar">ANALISAR</button><button class="pixel-button" data-report-action="resolver">RESOLVER</button></div>';container.appendChild(box);});
    }

    function renderPosts(container, items) {
        container.innerHTML = "";
        if (!items.length) { state(container, "NENHUMA PUBLICAÇÃO", "O backend não retornou publicações."); return; }
        items.forEach(function(item){
            var id=idOf(item), a=document.createElement("a"); a.className="post dynamic-item"; a.href="../post/index.html?id="+encodeURIComponent(id);
            a.innerHTML='<div class="post-head"><div class="row"><div class="avatar small">' + esc((item.autorNome||item.autor||item.usuario||"").slice(0,2).toUpperCase()) + '</div><div><strong>'+esc(item.autorNome||item.autor||item.usuario||"")+'</strong><br><span class="meta">'+esc(item.dataCriacao||item.criadoEm||"")+'</span></div></div></div><div class="post-body">'+esc(item.conteudo||item.texto||item.titulo||"")+'</div><div class="post-actions"><span>♥ '+esc(item.curtidas??"")+'</span><span>◉ '+esc(item.comentariosCount??"")+'</span></div>';
            container.appendChild(a);
        });
    }



    async function initCommunitySelect() {
        var select=document.getElementById("community-select"); if(!select)return;
        var data=await get("/comunidades",select); if(!data)return;
        var items=arr(data,["comunidades","items","content","data"]); select.innerHTML="";
        items.forEach(function(item){var o=document.createElement("option");o.value=idOf(item)||"";o.textContent=item.nome||"Comunidade";select.appendChild(o);});
    }

    async function initNotifications() {
        var root=document.getElementById("notifications-list"); if(!root) return;
        var data=await get("/notificacoes",root); if(!data)return;
        var items=arr(data,["notificacoes","items","content","data"]); root.innerHTML="";
        if(!items.length){state(root,"NENHUMA NOTIFICAÇÃO","O backend não retornou notificações.");return;}
        items.forEach(function(item){var n=document.createElement("div");n.className="notification"+(item.lida===false?" unread":"");n.dataset.id=idOf(item)||"";n.innerHTML='<div class="avatar small">'+esc((item.remetenteNome||item.autorNome||"").slice(0,2).toUpperCase())+'</div><div><strong>'+esc(item.titulo||item.remetenteNome||"")+'</strong> '+esc(item.mensagem||item.conteudo||"")+'<br><span class="meta">'+esc(item.criadoEm||item.dataCriacao||"")+'</span></div>';root.appendChild(n);});
        var mark=document.getElementById("mark-notifications"); if(mark) mark.onclick=async function(){try{await PrimataAPI.patch("/notificacoes/marcar-todas-como-lidas");root.querySelectorAll(".unread").forEach(function(x){x.classList.remove("unread")});}catch(e){mostrarErroAPI(e)}};
    }

    async function initPostComments() {
        var root=document.querySelector("[data-post-comments]"); if(!root)return;
        var id=new URLSearchParams(location.search).get("id"); if(!id)return;
        var data=await get("/publicacoes/"+encodeURIComponent(id)+"/comentarios",root); if(!data)return;
        var items=arr(data,["comentarios","items","content","data"]); root.innerHTML="";
        if(!items.length){root.innerHTML='<p class="meta">Nenhum comentário.</p>';}
        items.forEach(function(item){var c=document.createElement("div");c.className="comment";c.innerHTML='<div class="row"><div class="avatar small">'+esc((item.autorNome||item.autor||"").slice(0,2).toUpperCase())+'</div><div><strong>'+esc(item.autorNome||item.autor||"")+'</strong><span class="meta"> '+esc(item.criadoEm||item.dataCriacao||"")+'</span></div></div><p>'+esc(item.conteudo||"")+'</p>';root.appendChild(c);});
        var send=document.getElementById("send-comment"), input=document.getElementById("comment-input");
        if(send)send.onclick=async function(){if(!input||!input.value.trim())return;try{await PrimataAPI.post("/comentarios",{publicacaoId:id,conteudo:input.value.trim()});input.value="";await initPostComments();}catch(e){mostrarErroAPI(e)}};
    }

    async function initPostDetail() {
        var root=document.querySelector("[data-post-detail]"); if(!root) return;
        var id=new URLSearchParams(location.search).get("id"); if(!id){state(root,"PUBLICAÇÃO NÃO INFORMADA","Abra uma publicação retornada pelo backend.");return;}
        var data=await get("/publicacoes/"+encodeURIComponent(id),root); if(!data)return;
        root.innerHTML='<div class="post-head"><div class="row"><div class="avatar">'+esc((data.autorNome||data.autor||"").slice(0,2).toUpperCase())+'</div><div><strong>'+esc(data.autorNome||data.autor||"")+'</strong><span class="meta">'+esc(data.dataCriacao||data.criadoEm||"")+'</span></div></div></div><div class="post-body">'+esc(data.conteudo||data.texto||data.titulo||"")+'</div><div class="post-actions"><span>♥ '+esc(data.curtidas??"")+'</span><span>◉ '+esc(data.comentariosCount??"")+'</span></div>';
    }

    window.PrimataDados = { renderTracks: renderTracks, renderArtists: renderArtists, renderAlbums: renderAlbums, renderPlaylists: renderPlaylists, init: initListPage };




    async function initSessionHeader() {
        var button=document.querySelector(".header-button");
        if(!button) return;
        if(!localStorage.getItem("primataToken")) { button.textContent="ENTRAR"; button.href="../login/index.html"; return; }
        try {
            var data=await PrimataAPI.get("/usuarios/me");
            button.textContent=(data.nomeExibicao||data.username||"PERFIL").toUpperCase();
            button.href="../perfil/index.html";
        } catch(e) { button.textContent="ENTRAR"; button.href="../login/index.html"; }
    }

    async function initUserProfile() {
        var root=document.querySelector("[data-user-profile]"); if(!root) return;
        var data=await get("/usuarios/me",root); if(!data)return;
        root.innerHTML='<div class="row"><div class="avatar">'+esc((data.nomeExibicao||data.username||"").slice(0,2).toUpperCase())+'</div><div><span class="mini-title">ACCOUNT // PROFILE</span><h1>'+esc(data.nomeExibicao||data.username||"")+'</h1><p class="description">'+esc(data.bio||"")+'</p></div></div><div class="stats"><div><strong>'+esc(data.seguidoresCount??data.seguidores??"0")+'</strong><span>seguidores</span></div><div><strong>'+esc(data.seguindoCount??data.seguindo??"0")+'</strong><span>seguindo</span></div><div><strong>'+esc(data.playlistsCount??data.playlists??"0")+'</strong><span>playlists</span></div></div><div class="actions"><a class="pixel-button" href="../editar-perfil/index.html">EDITAR PERFIL</a></div>';
        var act=document.querySelector("[data-user-activity]"); if(act){var ad=await get("/usuarios/me/atividade",act);if(ad)renderPosts(act,arr(ad,["atividade","posts","items","content","data"]));}
    }

    async function initHomePlaylist() {
        var root=document.querySelector("[data-backend-playlist]");
        if(!root) return;
        var data=await get("/playlists/destaque",root);
        if(!data) return;
        root.innerHTML='<div class="playlist-art">'+cover(data,"PLAYLIST")+'</div><div class="playlist-info"><span class="tag">PLAYLIST</span><h3>'+esc(data.nome||data.titulo||"Playlist")+'</h3><p>'+esc(data.descricao||"")+'</p><div class="playlist-meta"><span>'+esc(data.quantidadeMusicas??"")+' MÚSICAS</span></div><button class="play-button" data-playlist-id="'+esc(idOf(data))+'">▶ OUVIR PLAYLIST</button></div>';
    }


    async function initCatalogTotals() {
        var total=document.getElementById("album-total"); if(!total)return;
        var data=await get("/albuns",total); if(!data)return;
        total.textContent=arr(data,["albuns","eps","items","content","data"]).length;
    }

    async function initLibrarySummary() {
        var ids=["count-artists","count-songs","count-albums","count-playlists"].map(function(x){return document.getElementById(x)});
        if(!ids.some(Boolean)) return;
        var root=document.querySelector(".library-stats"); var data=await get("/biblioteca/resumo",root); if(!data)return;
        var values=[data.artistas??data.artistasCount,data.musicas??data.musicasCount,data.albuns??data.albunsCount,data.playlists??data.playlistsCount];
        ids.forEach(function(e,i){if(e)e.textContent=values[i]??"0";});
    }

    async function initDashboards() {
        var roots=document.querySelectorAll("[data-backend-dashboard]");
        for(var i=0;i<roots.length;i++){
            var data=await get(roots[i].getAttribute("data-backend-dashboard"), roots[i]);
            if(data) roots[i].innerHTML='<pre class="backend-json">'+esc(JSON.stringify(data,null,2))+'</pre>';
        }
    }
    document.addEventListener("DOMContentLoaded", function () { initListPage(); initArtistDetail(); initMusicDetail(); initGenericTables(); initCommunities(); initPostDetail(); initDashboards(); initHomePlaylist(); initLibrarySummary(); initUserProfile(); initSessionHeader(); initNotifications(); initCatalogTotals(); initPostComments(); initCommunitySelect(); });
})();
