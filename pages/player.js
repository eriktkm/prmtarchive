/* PRIMATA ARCHIVE - PLAYER GLOBAL
   O player começa vazio. A música só aparece quando vier da API/ação do usuário.
*/
(function () {
    "use strict";
    var audio = null;
    var current = null;

    function ensureAudio() {
        if (!audio) {
            audio = document.createElement("audio");
            audio.id = "primata-audio";
            audio.preload = "metadata";
            document.body.appendChild(audio);
            audio.addEventListener("ended", function () { setPlaying(false); });
            audio.addEventListener("timeupdate", updateProgress);
        }
        return audio;
    }

    function setPlaying(value) {
        document.querySelectorAll(".main-play").forEach(function (b) { b.textContent = value ? "Ⅱ" : "▶"; });
    }

    function updateProgress() {
        var total = audio && audio.duration ? audio.duration : 0;
        var currentTime = audio ? audio.currentTime : 0;
        document.querySelectorAll(".player-progress span:first-child").forEach(function (e) { e.textContent = format(currentTime); });
        document.querySelectorAll(".player-progress span:last-child").forEach(function (e) { e.textContent = format(total); });
        document.querySelectorAll(".progress-bar > div").forEach(function (e) { e.style.width = total ? ((currentTime / total) * 100) + "%" : "0%"; });
    }

    function format(seconds) {
        if (!seconds || !isFinite(seconds)) return "00:00";
        var m = Math.floor(seconds / 60); var s = Math.floor(seconds % 60);
        return String(m).padStart(2, "0") + ":" + String(s).padStart(2, "0");
    }

    function updateUI(song) {
        document.querySelectorAll("#current-song, .player-song strong").forEach(function (e) { e.textContent = song.titulo || song.nome || ""; });
        document.querySelectorAll("#current-artist, .player-song span").forEach(function (e) { e.textContent = song.artista || song.artistaNome || ""; });
        var cover = song.capaUrl || song.coverUrl || song.imagemUrl;
        document.querySelectorAll(".player-cover").forEach(function (e) { e.innerHTML = cover ? '<img src="' + String(cover).replace(/\"/g, "&quot;") + '" alt="">' : ""; });
    }

    async function play(song) {
        if (!song || !song.audioUrl) { alert("Esta música não possui áudio disponível no backend."); return; }
        current = song; updateUI(song);
        var a = ensureAudio(); a.src = song.audioUrl; await a.play(); setPlaying(true);
    }

    function refresh() {
        document.querySelectorAll(".track-play").forEach(function (button) {
            if (button.dataset.playerBound) return;
            button.dataset.playerBound = "1";
            button.addEventListener("click", function (event) {
                event.preventDefault(); event.stopPropagation();
                var track = button.closest(".track"); if (!track) return;
                play({ titulo: track.querySelector(".track-data strong")?.textContent, artista: track.querySelector(".track-data span")?.textContent, audioUrl: track.dataset.audioUrl });
            });
        });
    }

    document.addEventListener("DOMContentLoaded", function () {
        refresh();
        document.querySelectorAll(".main-play").forEach(function (button) { button.addEventListener("click", async function () { var a = ensureAudio(); if (!current) return; if (a.paused) { await a.play(); setPlaying(true); } else { a.pause(); setPlaying(false); } }); });
    });

    window.PrimataPlayer = { play: play, refresh: refresh };
})();
