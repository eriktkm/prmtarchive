document.addEventListener("DOMContentLoaded", async function () {
    var container = document.getElementById("favorite-artists");
    if (!container) return;
    try {
        var data = await PrimataAPI.get("/biblioteca/artistas");
        var artists = Array.isArray(data) ? data : (data.artistas || data.content || []);
        if (!artists.length) {
            container.innerHTML = '<div class="backend-empty"><strong>NENHUM ARTISTA FAVORITO</strong><span>Quando você seguir ou favoritar um artista, ele aparecerá aqui.</span></div>';
            return;
        }
        container.innerHTML = artists.map(function (artist) {
            var id = artist.id != null ? encodeURIComponent(artist.id) : "";
            var name = escapeHtml(artist.nome || artist.name || artist.artista || "Artista");
            var genre = escapeHtml(artist.genero || artist.genre || "");
            var image = artist.capaUrl || artist.imagemUrl || artist.fotoUrl || "";
            return '<a class="favorite-artist" href="../perfil-artista/index.html?id=' + id + '">' +
                '<div class="favorite-artist-image">' + (image ? '<img src="' + escapeHtml(image) + '" alt="">' : '') + '</div>' +
                '<div><strong>' + name + '</strong>' + (genre ? '<span>' + genre + '</span>' : '') + '</div></a>';
        }).join("");
    } catch (error) {
        container.innerHTML = '<div class="backend-empty"><strong>BACKEND NÃO CONECTADO</strong><span>Endpoint esperado: GET /api/biblioteca/artistas</span></div>';
    }
});
function escapeHtml(value) {
    return String(value == null ? "" : value).replace(/[&<>"']/g, function (char) { return ({"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;","'":"&#039;"})[char]; });
}
