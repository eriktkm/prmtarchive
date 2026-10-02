document.addEventListener("DOMContentLoaded", async function () {
    const list = document.getElementById("personal-list");
    const search = document.getElementById("personal-search");
    let items = [];

    function escapeHtml(value) {
        return String(value ?? "").replace(/[&<>"']/g, function (c) {
            return ({ "&":"&amp;", "<":"&lt;", ">":"&gt;", '"':"&quot;", "'":"&#039;" })[c];
        });
    }

    function render(data) {
        list.innerHTML = "";
        if (!data.length) {
            list.innerHTML = '<div class="personal-empty"><h3>NENHUMA MÚSICA CURTIDA</h3><p>Quando o usuário curtir uma música, ela aparecerá aqui.</p></div>';
            return;
        }

        data.forEach(function (item, index) {
            const row = document.createElement("div");
            row.className = "personal-item";
            row.dataset.search = ((item.titulo || item.nome || "") + " " + (item.artista || "") + " " + (item.album || "")).toLowerCase();

            const cover = item.capaUrl
                ? '<img src="' + escapeHtml(item.capaUrl) + '" alt="">'
                : String(index + 1).padStart(2, "0");

            row.innerHTML =
                '<span class="track-number">' + String(index + 1).padStart(2, "0") + '</span>' +
                '<div class="personal-cover">' + cover + '</div>' +
                '<div class="personal-data">' +
                    '<strong>' + escapeHtml(item.titulo || item.nome || "Sem título") + '</strong>' +
                    '<span>' + escapeHtml(item.artista || "Artista") + (item.album ? " • " + escapeHtml(item.album) : "") + '</span>' +
                '</div>' +
                '<div class="personal-meta">' + escapeHtml(item.duracao || "--:--") + '<br>♥ CURTIDA</div>';

            if (item.audioUrl) row.dataset.audioUrl = item.audioUrl;
            list.appendChild(row);
        });
    }

    search.addEventListener("input", function () {
        const q = search.value.trim().toLowerCase();
        render(items.filter(function (item) {
            return ((item.titulo || item.nome || "") + " " + (item.artista || "") + " " + (item.album || "")).toLowerCase().includes(q);
        }));
    });

    try {
        const data = await PrimataAPI.get("/biblioteca/musicas");
        items = Array.isArray(data) ? data : (data.musicas || data.content || []);
        render(items);
    } catch (error) {
        list.innerHTML = '<div class="personal-empty"><h3>BACKEND NÃO CONECTADO</h3><p>Conecte o Spring Boot. Endpoint esperado: <strong>GET /api/biblioteca/musicas</strong>.</p></div>';
    }
});
