document.addEventListener("DOMContentLoaded", async function () {
    var lista = document.getElementById("library-list");
    try {
        var dados = await PrimataAPI.get("/biblioteca");
        var musicas = Array.isArray(dados) ? dados : (dados.musicas || []);
        if (!musicas.length) return;
        lista.innerHTML = "";
        musicas.forEach(function (musica, index) {
            lista.innerHTML += '<div class="track" data-audio-url="' + (musica.audioUrl || "") + '"><span class="track-number">' + String(index + 1).padStart(2, "0") + '</span><div class="track-cover">' + (index + 1) + '</div><div class="track-data"><strong>' + (musica.titulo || musica.nome) + '</strong><span>' + (musica.artista || "Artista") + '</span></div><span class="track-album">' + (musica.album || "-") + '</span><span class="track-time">' + (musica.duracao || "--:--") + '</span><button class="track-play">▶</button></div>';
        });
    } catch (error) {
        console.log("Biblioteca aguardando backend.");
    }
});
