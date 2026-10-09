document.addEventListener("DOMContentLoaded", function () {
    var audio = new Audio();
    var play = document.getElementById("play");
    var title = document.getElementById("player-title");
    var artist = document.getElementById("player-artist");

    window.addEventListener("primata:play", function (event) {
        var musica = event.detail || {};
        title.textContent = musica.titulo || musica.nome || "Música";
        artist.textContent = musica.artista || "Artista";
        if (musica.audioUrl) {
            audio.src = musica.audioUrl;
            audio.play().catch(function () {});
            play.textContent = "Ⅱ PAUSAR";
        }
    });

    play.addEventListener("click", function () {
        if (!audio.src) return;
        if (audio.paused) { audio.play(); play.textContent = "Ⅱ PAUSAR"; }
        else { audio.pause(); play.textContent = "▶ PLAY"; }
    });

    audio.addEventListener("timeupdate", function () {
        var atual = document.getElementById("player-current");
        var duracao = document.getElementById("player-duration");
        var fill = document.getElementById("player-fill");
        atual.textContent = formatar(audio.currentTime);
        duracao.textContent = formatar(audio.duration || 0);
        fill.style.width = audio.duration ? ((audio.currentTime / audio.duration) * 100) + "%" : "0%";
    });

    function formatar(segundos) {
        var min = Math.floor(segundos / 60).toString().padStart(2, "0");
        var sec = Math.floor(segundos % 60).toString().padStart(2, "0");
        return min + ":" + sec;
    }
});
