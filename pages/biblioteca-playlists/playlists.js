document.addEventListener("DOMContentLoaded", async function () {
    const grid = document.getElementById("personal-grid");
    const search = document.getElementById("personal-search");
    let items = [];

    function escapeHtml(value) {
        return String(value ?? "").replace(/[&<>"']/g, function (c) {
            return ({ "&":"&amp;", "<":"&lt;", ">":"&gt;", '"':"&quot;", "'":"&#039;" })[c];
        });
    }

    function render(data) {
        grid.innerHTML = "";
        if (!data.length) {
            grid.innerHTML = '<div class="personal-empty"><h3>NENHUMA PLAYLIST</h3><p>Crie sua primeira playlist para ela aparecer aqui.</p></div>';
            return;
        }

        data.forEach(function (item, index) {
            const card = document.createElement("a");
            card.className = "personal-card";
            card.href = item.url || ("../playlist-usuario/index.html?id=" + encodeURIComponent(item.id ?? ""));
            const cover = item.capaUrl
                ? '<img src="' + escapeHtml(item.capaUrl) + '" alt="">'
                : "PL " + String(index + 1).padStart(2, "0");

            card.dataset.search = ((item.nome || item.titulo || "") + " " + (item.descricao || "")).toLowerCase();
            card.innerHTML =
                '<div class="personal-card-cover">' + cover + '</div>' +
                '<h3>' + escapeHtml(item.nome || item.titulo || "Playlist") + '</h3>' +
                '<p>' + escapeHtml(item.descricao || ((item.quantidadeMusicas ?? item.musicas?.length ?? 0) + " músicas")) + '</p>';
            grid.appendChild(card);
        });
    }

    search.addEventListener("input", function () {
        const q = search.value.trim().toLowerCase();
        render(items.filter(function (item) {
            return ((item.nome || item.titulo || "") + " " + (item.descricao || "")).toLowerCase().includes(q);
        }));
    });

    try {
        const data = await PrimataAPI.get("/biblioteca/playlists");
        items = Array.isArray(data) ? data : (data.playlists || data.content || []);
        render(items);
    } catch (error) {
        grid.innerHTML = '<div class="personal-empty"><h3>BACKEND NÃO CONECTADO</h3><p>Conecte o Spring Boot. Endpoint esperado: <strong>GET /api/biblioteca/playlists</strong>.</p></div>';
    }
});
