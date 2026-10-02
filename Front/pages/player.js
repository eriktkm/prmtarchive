/* PRIMATA ARCHIVE - PLAYER GLOBAL
   O player só é criado/exibido quando uma música realmente começa a tocar.
*/
(function () {
    "use strict";
    var audio = null;
    var current = null;
    var player = null;
    var bound = false;

    function createPlayer() {
        if (player) return player;
        player = document.createElement("footer");
        player.className = "player";
        player.id = "primata-player";
        player.innerHTML = `
            <div class="player-song">
                <div class="player-cover"></div>
                <div>
                    <strong id="current-song"></strong>
                    <span id="current-artist"></span>
                </div>
            </div>
            <div class="player-controls">
                <button type="button" class="player-prev" aria-label="Anterior">↶</button>
                <button type="button" class="main-play" aria-label="Pausar">Ⅱ</button>
                <button type="button" class="player-next" aria-label="Próxima">↷</button>
            </div>
            <div class="player-progress">
                <span>00:00</span>
                <div class="progress-bar"><div></div></div>
                <span>00:00</span>
            </div>
        `;
        document.body.appendChild(player);

        player.querySelector(".main-play").addEventListener("click", async function () {
            var a = ensureAudio();
            if (!current) return;
            if (a.paused) {
                try { await a.play(); } catch (e) { hidePlayer(); }
            } else {
                a.pause();
            }
        });

        player.querySelector(".player-prev").addEventListener("click", function () {});
        player.querySelector(".player-next").addEventListener("click", function () {});
        return player;
    }

    function showPlayer() {
        createPlayer().classList.add("is-visible");
    }

    function hidePlayer() {
        if (player) player.classList.remove("is-visible");
    }

    function ensureAudio() {
        if (!audio) {
            audio = document.createElement("audio");
            audio.id = "primata-audio";
            audio.preload = "metadata";
            audio.style.display = "none";
            document.body.appendChild(audio);
            audio.addEventListener("play", function () { showPlayer(); setPlaying(true); });
            audio.addEventListener("pause", function () { setPlaying(false); hidePlayer(); });
            audio.addEventListener("ended", function () { setPlaying(false); hidePlayer(); });
            audio.addEventListener("timeupdate", updateProgress);
        }
        return audio;
    }

    function setPlaying(value) {
        if (!player) return;
        var button = player.querySelector(".main-play");
        if (button) button.textContent = value ? "Ⅱ" : "▶";
    }

    function updateProgress() {
        if (!player || !audio) return;
        var total = audio.duration || 0;
        var currentTime = audio.currentTime || 0;
        var times = player.querySelectorAll(".player-progress span");
        if (times[0]) times[0].textContent = format(currentTime);
        if (times[1]) times[1].textContent = format(total);
        var bar = player.querySelector(".progress-bar > div");
        if (bar) bar.style.width = total ? ((currentTime / total) * 100) + "%" : "0%";
    }

    function format(seconds) {
        if (!seconds || !isFinite(seconds)) return "00:00";
        var m = Math.floor(seconds / 60);
        var s = Math.floor(seconds % 60);
        return String(m).padStart(2, "0") + ":" + String(s).padStart(2, "0");
    }

    function updateUI(song) {
        var p = createPlayer();
        var title = p.querySelector("#current-song");
        var artist = p.querySelector("#current-artist");
        var coverEl = p.querySelector(".player-cover");
        if (title) title.textContent = song.titulo || song.nome || "";
        if (artist) artist.textContent = song.artista || song.artistaNome || "";
        var cover = song.capaUrl || song.coverUrl || song.imagemUrl;
        if (coverEl) coverEl.innerHTML = cover ? '<img src="' + String(cover).replace(/\"/g, "&quot;") + '" alt="">' : "";
    }

    async function play(song) {
        if (!song || !song.audioUrl) {
            alert("Esta música não possui áudio disponível no backend.");
            return;
        }
        current = song;
        updateUI(song);
        var a = ensureAudio();
        a.src = song.audioUrl;
        try {
            await a.play();
            showPlayer();
            setPlaying(true);
        } catch (error) {
            hidePlayer();
            alert("Não foi possível reproduzir esta música.");
        }
    }

    function refresh() {
        document.querySelectorAll(".track-play").forEach(function (button) {
            if (button.dataset.playerBound) return;
            button.dataset.playerBound = "1";
            button.addEventListener("click", function (event) {
                event.preventDefault();
                event.stopPropagation();
                var track = button.closest(".track");
                if (!track) return;
                play({
                    titulo: track.querySelector(".track-data strong")?.textContent || "",
                    artista: track.querySelector(".track-data span")?.textContent || "",
                    audioUrl: track.dataset.audioUrl,
                    capaUrl: track.dataset.coverUrl
                });
            });
        });
    }

    document.addEventListener("DOMContentLoaded", function () {
        refresh();
    });

    window.PrimataPlayer = { play: play, refresh: refresh };
})();
