(function () {
    var base = "../";

    var grupos = [
        {
            titulo: "EXPLORAR",
            links: [
                ["HOME", "landing/index.html"],
                ["EXPLORAR", "explorar/index.html"],
                ["ARTISTAS", "artistas/index.html"],
                ["MÚSICAS", "musicas/index.html"],
                ["ÁLBUNS", "albuns/index.html"],
                ["PLAYLISTS", "playlist/index.html"],
                ["BIBLIOTECA", "biblioteca/index.html"],
                ["PLAYER EXPANDIDO", "player-expandido/index.html"],
                ["BUSCAR", "resultados-pesquisa/index.html"]
            ]
        },
        {
            titulo: "COMUNIDADE",
            links: [
                ["COMUNIDADE", "comunidade/index.html"],
                ["NOVA COMUNIDADE", "criar-comunidade/index.html"],
                ["NOVA PUBLICAÇÃO", "criar-publicacao/index.html"],
                ["COMUNIDADE ESPECÍFICA", "comunidade-especifica/index.html"],
                ["POST", "post/index.html"]
            ]
        },
        {
            titulo: "MINHA CONTA",
            links: [
                ["MEU PERFIL", "perfil/index.html"],
                ["PERFIL DE ARTISTA", "perfil-artista/index.html"],
                ["EDITAR PERFIL", "editar-perfil/index.html"],
                ["NOTIFICAÇÕES", "notificacoes/index.html"],
                ["CONFIGURAÇÕES", "configuracoes/index.html"]
            ]
        },
        {
            titulo: "ARTISTA",
            links: [
                ["DASHBOARD", "dashboard-artista/index.html"],
                ["GERENCIAR MÚSICAS", "gerenciar-musicas/index.html"],
                ["CRIAR LANÇAMENTO", "criar-lancamento/index.html"],
                ["ESTATÍSTICAS", "estatisticas-artista/index.html"]
            ]
        },
        {
            titulo: "ADMINISTRAÇÃO",
            links: [
                ["DASHBOARD ADMIN", "dashboard-administrativo/index.html"],
                ["USUÁRIOS", "gerenciar-usuarios/index.html"],
                ["CONTEÚDO", "gerenciar-conteudo/index.html"],
                ["DENÚNCIAS", "denuncias/index.html"],
                ["COMUNIDADES", "gerenciar-comunidades/index.html"]
            ]
        }
    ];

    function criarMenu() {
        var main = document.querySelector("main");
        if (!main || document.querySelector(".primata-access-menu")) return;

        var section = document.createElement("section");
        section.className = "section primata-access-menu";
        section.innerHTML = '<div class="section-header"><div><span class="section-code">NAV_001</span><h2>ACESSOS DO SISTEMA</h2></div></div>';

        var wrapper = document.createElement("div");
        wrapper.className = "nav-access-grid";

        grupos.forEach(function (grupo) {
            var box = document.createElement("div");
            box.className = "panel nav-access-box";
            box.innerHTML = '<span class="code-label">' + grupo.titulo + '</span>';

            var links = document.createElement("div");
            links.className = "nav-access-links";

            grupo.links.forEach(function (item) {
                var a = document.createElement("a");
                a.className = "pixel-button alt nav-access-button";
                a.href = base + item[1];
                a.textContent = item[0];
                links.appendChild(a);
            });

            box.appendChild(links);
            wrapper.appendChild(box);
        });

        section.appendChild(wrapper);
        main.insertBefore(section, main.firstElementChild);
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", criarMenu);
    } else {
        criarMenu();
    }
})();
