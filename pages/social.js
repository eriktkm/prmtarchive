/* PRIMATA ARCHIVE - FUNÇÕES SOCIAIS CONECTADAS À API */

document.addEventListener("DOMContentLoaded", function () {
    iniciarPlayerSocial();
    iniciarComentarios();
    iniciarComunidades();
    iniciarPublicacao();
    iniciarCriacaoComunidade();
    iniciarEdicaoPerfil();
    iniciarNotificacoes();
});

function iniciarPlayerSocial() {
    var botoes = document.querySelectorAll(".player .pixel-button");
    var tocar = false;

    if (botoes.length) {
        var principal = document.querySelector(".player .pixel-button:not(.alt)");
        if (principal) {
            principal.addEventListener("click", function () {
                tocar = !tocar;
                principal.textContent = tocar ? "Ⅱ" : "▶";
            });
        }
    }
}

function iniciarComentarios() {
    document.querySelectorAll(".actions").forEach(function (actions) {
        var button = Array.from(actions.querySelectorAll("button")).find(function (item) {
            return item.textContent.trim() === "ENVIAR";
        });

        if (!button) return;

        button.addEventListener("click", async function () {
            var input = actions.querySelector("input");
            if (!input || !input.value.trim()) {
                alert("Digite um comentário antes de enviar.");
                return;
            }

            try {
                await PrimataAPI.post("/comentarios", {
                    conteudo: input.value.trim(),
                    postId: obterIdDaURL("postId")
                });

                input.value = "";
                alert("Comentário enviado!");
            } catch (error) {
                mostrarErroAPI(error);
            }
        });
    });
}

function iniciarComunidades() {
    document.querySelectorAll("button").forEach(function (button) {
        if (button.textContent.trim() !== "ENTRAR NA COMUNIDADE") return;

        button.addEventListener("click", async function () {
            var id = obterIdDaURL("id");
            if (!id) {
                alert("Esta comunidade precisa de um ID vindo do backend.");
                return;
            }

            try {
                await PrimataAPI.post("/comunidades/" + id + "/entrar");
                button.textContent = "✓ MEMBRO DA COMUNIDADE";
                button.disabled = true;
            } catch (error) {
                mostrarErroAPI(error);
            }
        });
    });
}

function iniciarPublicacao() {
    var pagina = window.location.pathname.toLowerCase();
    if (!pagina.includes("criar-publicacao")) return;

    var campos = document.querySelectorAll(".field");
    var select = campos[0] ? campos[0].querySelector("select") : null;
    var inputs = document.querySelectorAll("input");
    var textarea = document.querySelector("textarea");
    var publicar = localizarBotao("PUBLICAR");
    var rascunho = localizarBotao("SALVAR RASCUNHO");

    if (publicar) publicar.addEventListener("click", function () { enviarPublicacao("PUBLICADA"); });
    if (rascunho) rascunho.addEventListener("click", function () { enviarPublicacao("RASCUNHO"); });

    async function enviarPublicacao(status) {
        var titulo = inputs[0] ? inputs[0].value.trim() : "";
        var conteudo = textarea ? textarea.value.trim() : "";
        var link = inputs[1] ? inputs[1].value.trim() : "";

        if (!titulo || !conteudo) {
            alert("Preencha título e conteúdo.");
            return;
        }

        try {
            await PrimataAPI.post("/publicacoes", {
                comunidadeId: select ? select.value : null,
                titulo: titulo,
                conteudo: conteudo,
                anexoUrl: link || null,
                status: status
            });
            alert(status === "RASCUNHO" ? "Rascunho salvo!" : "Publicação criada!");
        } catch (error) {
            mostrarErroAPI(error);
        }
    }
}

function iniciarCriacaoComunidade() {
    var pagina = window.location.pathname.toLowerCase();
    if (!pagina.includes("criar-comunidade")) return;

    var inputs = document.querySelectorAll("input");
    var textarea = document.querySelector("textarea");
    var selects = document.querySelectorAll("select");
    var button = localizarBotao("CRIAR COMUNIDADE");

    if (!button) return;

    button.addEventListener("click", async function () {
        if (!inputs[0] || !inputs[0].value.trim() || !inputs[1] || !inputs[1].value.trim()) {
            alert("Preencha o nome e o identificador.");
            return;
        }

        try {
            await PrimataAPI.post("/comunidades", {
                nome: inputs[0].value.trim(),
                identificador: inputs[1].value.trim(),
                descricao: textarea ? textarea.value.trim() : "",
                categoria: selects[0] ? selects[0].value : null,
                privacidade: selects[1] ? selects[1].value : null
            });
            window.location.href = "../comunidade/index.html";
        } catch (error) {
            mostrarErroAPI(error);
        }
    });
}

function iniciarEdicaoPerfil() {
    var pagina = window.location.pathname.toLowerCase();
    if (!pagina.includes("editar-perfil")) return;

    var inputs = document.querySelectorAll("input");
    var textarea = document.querySelector("textarea");
    var select = document.querySelector("select");
    var button = localizarBotao("SALVAR ALTERAÇÕES");

    if (!button) return;

    button.addEventListener("click", async function () {
        try {
            await PrimataAPI.put("/usuarios/me", {
                username: inputs[0] ? inputs[0].value.trim() : "",
                nomeExibicao: inputs[1] ? inputs[1].value.trim() : "",
                bio: textarea ? textarea.value.trim() : "",
                generoFavorito: select ? select.value : null
            });
            alert("Perfil atualizado!");
        } catch (error) {
            mostrarErroAPI(error);
        }
    });
}

function iniciarNotificacoes() {
    var button = localizarBotao("MARCAR TODAS COMO LIDAS");
    if (!button) return;

    button.addEventListener("click", async function () {
        try {
            await PrimataAPI.patch("/notificacoes/marcar-todas-como-lidas");
            document.querySelectorAll(".notification.unread").forEach(function (item) {
                item.classList.remove("unread");
            });
        } catch (error) {
            mostrarErroAPI(error);
        }
    });
}

function localizarBotao(texto) {
    return Array.from(document.querySelectorAll("button")).find(function (button) {
        return button.textContent.trim() === texto;
    });
}

function obterIdDaURL(nome) {
    return new URLSearchParams(window.location.search).get(nome);
}
