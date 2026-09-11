/* PRIMATA ARCHIVE - PLAYER */

document.addEventListener("DOMContentLoaded", function () {
    var trackButtons = document.querySelectorAll(".track-play");
    var currentSong = document.getElementById("current-song");
    var currentArtist = document.getElementById("current-artist");
    var mainPlay = document.querySelector(".main-play");
    var currentTrack = null;
    var playing = false;

    function tocarMusica(button) {
        var track = button.closest(".track");
        if (!track) return;

        var songElement = track.querySelector(".track-data strong");
        var artistElement = track.querySelector(".track-data span");
        var audioUrl = track.getAttribute("data-audio-url");

        if (!songElement || !artistElement) return;

        if (currentSong) currentSong.textContent = songElement.textContent;
        if (currentArtist) currentArtist.textContent = artistElement.textContent;

        trackButtons.forEach(function (btn) {
            btn.textContent = "▶";
        });

        button.textContent = "Ⅱ";
        currentTrack = track;
        playing = true;
        if (mainPlay) mainPlay.textContent = "Ⅱ";

        if (audioUrl) {
            tocarAudioReal(audioUrl);
        }
    }

    function tocarAudioReal(url) {
        var audio = document.getElementById("primata-audio");
        if (!audio) {
            audio = document.createElement("audio");
            audio.id = "primata-audio";
            document.body.appendChild(audio);
        }

        audio.src = url;
        audio.play().catch(function () {});
    }

    trackButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            tocarMusica(button);
        });
    });

    if (mainPlay) {
        mainPlay.addEventListener("click", function () {
            if (!currentTrack) {
                var first = document.querySelector(".track-play");
                if (first) tocarMusica(first);
                return;
            }

            var audio = document.getElementById("primata-audio");
            playing = !playing;
            mainPlay.textContent = playing ? "Ⅱ" : "▶";

            if (audio) {
                if (playing) audio.play().catch(function () {});
                else audio.pause();
            }
        });
    }

    var playerButtons = document.querySelectorAll(".player-controls button");
    if (playerButtons.length >= 3) {
        playerButtons[0].addEventListener("click", function () {
            mudarFaixa(-1);
        });

        playerButtons[2].addEventListener("click", function () {
            mudarFaixa(1);
        });
    }

    function mudarFaixa(direcao) {
        var tracks = Array.from(document.querySelectorAll(".track"));
        if (!tracks.length || !currentTrack) return;

        var index = tracks.indexOf(currentTrack);
        var novoIndex = index + direcao;

        if (novoIndex >= 0 && novoIndex < tracks.length) {
            var button = tracks[novoIndex].querySelector(".track-play");
            if (button) tocarMusica(button);
        }
    }
});
