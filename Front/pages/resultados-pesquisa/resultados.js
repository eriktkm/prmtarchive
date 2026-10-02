document.addEventListener("DOMContentLoaded", function () {
    var form = document.getElementById("search-form");
    var input = document.getElementById("search-input");
    var results = document.getElementById("results");
    var title = document.getElementById("result-title");

    var parametroInicial = new URLSearchParams(window.location.search).get("q");
    if (parametroInicial) {
        input.value = parametroInicial;
        setTimeout(function () {
            form.requestSubmit();
        }, 0);
    }

    form.addEventListener("submit", async function (event) {
        event.preventDefault();
        var termo = input.value.trim();
        if (!termo) return;
        title.textContent = "BUSCA: " + termo.toUpperCase();
        results.innerHTML = '<div class="pixel-card"><span class="code-label">SEARCH</span><h3>CARREGANDO...</h3><p>Consultando o backend.</p></div>';

        try {
            var dados = await PrimataAPI.get("/busca?q=" + encodeURIComponent(termo));
            renderizar(dados);
        } catch (error) {
            results.innerHTML = '<div class="pixel-card"><span class="code-label">API</span><h3>NENHUM RESULTADO CARREGADO</h3><p>Verifique se o endpoint /busca existe no Spring Boot.</p></div>';
        }
    });

    function renderizar(dados) {
        var lista = Array.isArray(dados) ? dados : (dados.resultados || []);
        results.innerHTML = "";
        if (!lista.length) {
            results.innerHTML = '<div class="pixel-card"><h3>NADA ENCONTRADO</h3><p>Tente outro termo.</p></div>';
            return;
        }
        lista.forEach(function (item) {
            var card = document.createElement("a");
            card.className = "pixel-card";
            card.href = item.url || "../musicas/index.html";
            card.innerHTML = '<span class="code-label">RESULTADO</span><h3>' + (item.nome || item.titulo || "Sem título") + '</h3><p>' + (item.artista || item.tipo || "Primata Archive") + '</p>';
            results.appendChild(card);
        });
    }
});
