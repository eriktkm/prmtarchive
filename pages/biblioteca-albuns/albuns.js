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
            grid.innerHTML = '<div class="personal-empty"><h3>NENHUM ÁLBUM OU EP CURTIDO</h3><p>Quando você curtir um lançamento, ele aparecerá aqui.</p></div>';
            return;
        }

        data.forEach(function (item, index) {
            const card = document.createElement("a");
            card.className = "personal-card";
            card.href = item.url || ("../albuns/index.html?id=" + encodeURIComponent(item.id ?? ""));
            const cover = item.capaUrl
                ? '<img src="' + escapeHtml(item.capaUrl) + '" alt="">'
                : "RE " + String(index + 1).padStart(2, "0");

            card.dataset.search = ((item.titulo || item.nome || "") + " " + (item.artista || "")).toLowerCase();
            card.innerHTML =
                '<div class="personal-card-cover">' + cover + '</div>' +
                '<h3>' + escapeHtml(item.titulo || item.nome || "Lançamento") + '</h3>' +
                '<p>' + escapeHtml(item.artista || "Artista") + ' • ' + escapeHtml(item.tipo || "ÁLBUM / EP") + '</p>';
            grid.appendChild(card);
        });
    }

    search.addEventListener("input", function () {
        const q = search.value.trim().toLowerCase();
        render(items.filter(function (item) {
            return ((item.titulo || item.nome || "") + " " + (item.artista || "")).toLowerCase().includes(q);
        }));
    });

    try {
        const data = await PrimataAPI.get("/biblioteca/albuns");
        items = Array.isArray(data) ? data : (data.albuns || data.eps || data.content || []);
        render(items);
    } catch (error) {
        grid.innerHTML = '<div class="personal-empty"><h3>BACKEND NÃO CONECTADO</h3><p>Conecte o Spring Boot. Endpoint esperado: <strong>GET /api/biblioteca/albuns</strong>.</p></div>';
    }
});
