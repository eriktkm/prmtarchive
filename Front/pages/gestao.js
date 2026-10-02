/* PRIMATA ARCHIVE - GESTÃO CONECTADA AO SPRING BOOT */

document.addEventListener("DOMContentLoaded", function () {
    iniciarPlayerGestao();
    iniciarConfiguracoes();
    iniciarLancamento();
    iniciarAcoesGestao();
    iniciarFiltros();
});

function iniciarPlayerGestao() {
    var mainPlay = document.querySelector(".main-play");
    if (!mainPlay) return;

    var playing = false;
    mainPlay.addEventListener("click", function () {
        playing = !playing;
        mainPlay.textContent = playing ? "Ⅱ" : "▶";
    });
}

function iniciarConfiguracoes() {
    var pagina = window.location.pathname.toLowerCase();
    if (!pagina.includes("configuracoes")) return;

    var inputs = document.querySelectorAll("input");
    var selects = document.querySelectorAll("select");
    var button = localizarBotao("SALVAR ALTERAÇÕES");

    if (button) {
        button.addEventListener("click", async function () {
            try {
                await PrimataAPI.put("/usuarios/me", {
                    username: inputs[0] ? inputs[0].value.trim() : "",
                    email: inputs[1] ? inputs[1].value.trim() : "",
                    idioma: selects[0] ? selects[0].value : null,
                    fusoHorario: selects[1] ? selects[1].value : null
                });
                alert("Configurações salvas!");
            } catch (error) {
                mostrarErroAPI(error);
            }
        });
    }

    document.querySelectorAll(".switch").forEach(function (switchElement) {
        switchElement.addEventListener("click", async function () {
            switchElement.classList.toggle("on");

            var row = switchElement.closest(".switch-row");
            var titulo = row ? row.querySelector("strong") : null;

            try {
                await PrimataAPI.patch("/usuarios/me/preferencias", {
                    chave: titulo ? titulo.textContent.trim() : "",
                    valor: switchElement.classList.contains("on")
                });
            } catch (error) {
                switchElement.classList.toggle("on");
                mostrarErroAPI(error);
            }
        });
    });
}

function iniciarLancamento() {
    var pagina = window.location.pathname.toLowerCase();
    if (!pagina.includes("criar-lancamento")) return;

    var inputs = document.querySelectorAll("input");
    var selects = document.querySelectorAll("select");
    var textarea = document.querySelector("textarea");
    var botoes = document.querySelectorAll("button");

    botoes.forEach(function (button) {
        var texto = button.textContent.trim();
        if (texto !== "SALVAR RASCUNHO" && texto !== "PUBLICAR") return;

        button.addEventListener("click", async function () {
            var titulo = inputs[0] ? inputs[0].value.trim() : "";
            if (!titulo) {
                alert("Informe o título do lançamento.");
                return;
            }

            var formData = new FormData();
            formData.append("titulo", titulo);
            formData.append("tipo", selects[0] ? selects[0].value : "");
            formData.append("dataLancamento", inputs[1] ? inputs[1].value : "");
            formData.append("genero", selects[1] ? selects[1].value : "");
            formData.append("descricao", textarea ? textarea.value : "");
            formData.append("status", texto === "PUBLICAR" ? "PUBLICADO" : "RASCUNHO");

            if (inputs[3] && inputs[2].files[0]) formData.append("capa", inputs[2].files[0]);
            if (inputs[4] && inputs[3].files[0]) formData.append("audio", inputs[3].files[0]);

            try {
                await PrimataAPI.upload("/lancamentos", formData);
                alert(texto === "PUBLICAR" ? "Lançamento publicado!" : "Rascunho salvo!");
            } catch (error) {
                mostrarErroAPI(error);
            }
        });
    });
}

function iniciarAcoesGestao() {
    document.querySelectorAll(".demo-action").forEach(function (button) {
        button.classList.remove("demo-action");
        if (button.dataset.apiReady) return;
        button.dataset.apiReady = "true";

        button.addEventListener("click", async function () {
            var texto = button.textContent.trim();
            var row = button.closest("tr, .pixel-card, .community-item");
            var id = row ? row.getAttribute("data-id") : null;

            if (!id) {
                alert("Esta ação precisa do ID do registro enviado pelo backend.");
                return;
            }

            var config = obterAcao(texto);
            if (!config) return;

            if (config.confirm && !confirm(config.confirm)) return;

            try {
                if (config.method === "GET") await PrimataAPI.get(config.endpoint + id);
                if (config.method === "PATCH") await PrimataAPI.patch(config.endpoint + id);
                if (config.method === "DELETE") await PrimataAPI.delete(config.endpoint + id);
                alert(config.success);
            } catch (error) {
                mostrarErroAPI(error);
            }
        });
    });
}

function obterAcao(texto) {
    var acoes = {
        "VER": { method: "GET", endpoint: "/usuarios/", success: "Usuário carregado." },
        "EDITAR": { method: "GET", endpoint: "/musicas/", success: "Registro carregado para edição." },
        "ANALISAR": { method: "PATCH", endpoint: "/conteudos/", success: "Conteúdo enviado para análise." },
        "ARQUIVAR": { method: "PATCH", endpoint: "/denuncias/", success: "Denúncia arquivada.", confirm: "Arquivar esta denúncia?" },
        "RESOLVER": { method: "PATCH", endpoint: "/denuncias/", success: "Denúncia resolvida.", confirm: "Marcar esta denúncia como resolvida?" },
        "VER CONTEÚDO": { method: "GET", endpoint: "/denuncias/conteudo/", success: "Conteúdo carregado." },
        "GERENCIAR": { method: "GET", endpoint: "/comunidades/", success: "Comunidade carregada." }
    };

    return acoes[texto] || null;
}

function iniciarFiltros() {
    document.querySelectorAll(".filter-bar").forEach(function (bar) {
        var input = bar.querySelector("input");
        var selects = bar.querySelectorAll("select");
        var panel = bar.closest(".panel");
        if (!panel) return;

        function filtrar() {
            var termo = input ? input.value.toLowerCase().trim() : "";
            var status = selects.length ? selects[0].value.toLowerCase() : "";

            panel.querySelectorAll("tbody tr, .community-item, .pixel-card").forEach(function (item) {
                var texto = item.textContent.toLowerCase();
                var okTexto = !termo || texto.includes(termo);
                var okStatus = !status || status.includes("todos") || texto.includes(status);
                item.style.display = okTexto && okStatus ? "" : "none";
            });
        }

        if (input) input.addEventListener("input", filtrar);
        selects.forEach(function (select) { select.addEventListener("change", filtrar); });
    });
}

function localizarBotao(texto) {
    return Array.from(document.querySelectorAll("button")).find(function (button) {
        return button.textContent.trim() === texto;
    });
}
